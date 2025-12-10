using Newtonsoft.Json;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Models.Auth;
using sistecDesktopRefactored.Models.Dashboard;
using sistecDesktopRefactored.Models.IA;
using sistecDesktopRefactored.Models.Tickets;
using sistecDesktopRefactored.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Services
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;        // objeto para fazer as requisições HTTP
        private readonly CookieContainer _cookieContainer;      // gerencia os cookies para autenticar automaticamente
        private readonly string _baseUrl = Environment.GetEnvironmentVariable("BASE_URL");     // URL base da API

        // Configurações para serialização/deserialização JSON (ignora nulos, datas no formato ISO)
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        #region ApiRequestHelpers

        private async Task<TResponse> GetApiAsync<TResponse>(string relativeUrl)
        {
            try
            {
                var url = $"{_baseUrl}{relativeUrl}";
                Console.WriteLine($"DEBUG: GET {url}");

                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine("=== JSON COMPLETO ===");
                Console.WriteLine(responseBody);
                Console.WriteLine("=====================");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TResponse>(responseBody, _jsonSettings);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"DEBUG: Falha ao deserializar {typeof(TResponse).Name}: {ex.Message}");
                        throw new Exception($"Erro ao deserializar resposta: {ex.Message}");
                    }
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                }
                throw new Exception($"Erro na requisição: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        private async Task<TResponse> PostApiAsync<TRequest, TResponse>(string relativeUrl, TRequest body)
        {
            try
            {
                var url = $"{_baseUrl}{relativeUrl}";
                var json = JsonConvert.SerializeObject(body, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"DEBUG: POST {url}");
                Console.WriteLine($"Body: {json}");

                var response = await _httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine("=== JSON COMPLETO ===");
                Console.WriteLine(responseBody);
                Console.WriteLine("=====================");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TResponse>(responseBody, _jsonSettings);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"DEBUG: Falha ao deserializar {typeof(TResponse).Name}: {ex.Message}");
                        throw new Exception($"Erro ao deserializar resposta: {ex.Message}");
                    }
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");

                throw new Exception($"Erro na requisição: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the <see cref="HttpClient"/> with a default configuration
        /// that includes a <see cref="CookieContainer"/> for managing cookies and sets the default request headers to
        /// accept JSON responses. The default timeout for requests is set to 30 seconds.</remarks>
        public ApiClient()
        {
            _cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler()
            {
                CookieContainer = _cookieContainer,
                UseCookies = true
            };

            _httpClient = new HttpClient(handler);      // Associa esse handler ao HttpClient principal                                                
            _httpClient.DefaultRequestHeaders.Accept.Clear();        // Limpa configurações de Accept
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));       // Define apenas respostas no formato JSON

            // Define timeout padrão das requisições pra 30 segundos
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
        #endregion

        #region Teste de Conexão
        /// <summary>
        /// Tests the connection to the configured base URL by sending a GET request.
        /// </summary>
        /// <remarks>This method attempts to connect to the base URL using an HTTP GET request and returns
        /// an <see cref="ApiResponse"/> indicating the success or failure of the connection attempt.</remarks>
        /// <returns>An <see cref="ApiResponse"/> object containing the result of the connection test. The <see
        /// cref="ApiResponse.Success"/> property is <see langword="true"/> if the connection is successful; otherwise,
        /// <see langword="false"/>. The <see cref="ApiResponse.Message"/> property contains a message describing the
        /// result.</returns>
        public async Task<ApiResponse> TestConnection()
        {
            try
            {
                Console.WriteLine($"DEBUG: Testando conexão: {_baseUrl}");
                var response = await _httpClient.GetAsync($"{_baseUrl}/");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content.Substring(0, Math.Min(content.Length, 100))}...");

                return new ApiResponse
                {
                    Success = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? "Conexão OK" : "Falha na conexão"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO na conexão: {ex.Message}");
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Login e Logout
        /// <summary>
        /// Attempts to log in a user asynchronously using the provided login request data.
        /// </summary>
        /// <remarks>This method sends a POST request to the authentication endpoint with the login
        /// details serialized as JSON. It handles HTTP response codes to determine the success of the login attempt and
        /// captures any exceptions that occur during the process.</remarks>
        /// <param name="loginRequest">The login request containing user credentials and other necessary information.</param>
        /// <returns>A <see cref="LoginResponse"/> object indicating the success or failure of the login attempt. If successful,
        /// it contains user data and authentication details; otherwise, it includes an error message.</returns>
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                // Serializa o objeto loginRequest pra JSON
                var json = JsonConvert.SerializeObject(loginRequest, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Monta a URL do endpoint de login
                var loginUrl = $"{_baseUrl}/api/auth/login";

                Console.WriteLine($"DEBUG: Fazendo login para: {loginUrl}");
                Console.WriteLine($"Dados: {json}");

                // Envia a requisição POST para o endpoint de login com o JSON
                var response = await _httpClient.PostAsync(loginUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Resposta completa: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta for 2xx, deserializa para LoginResponse
                    Console.WriteLine("JSON recebido:");
                    Console.WriteLine(responseBody);
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody, _jsonSettings);

                    Console.WriteLine($"DEBUG: User ID deserializado: {loginResponse.Data?.User?.IdPerfilUsuario.Id}");
                    Console.WriteLine($"DEBUG: User Name deserializado: {loginResponse.Data?.User?.NomeUsuario}");
                    Console.WriteLine($"DEBUG: Perfil Nivel deserializado: {loginResponse.Data?.User?.IdPerfilUsuario.NivelAcesso}");

                    if (loginResponse.Success)
                    {
                        // Se o login foi aceito, mostra quantos cookies foram recebidos
                        Console.WriteLine($"Cookies recebidos: {_cookieContainer.Count}");
                        var uri = new Uri(_baseUrl);
                        foreach (Cookie cookie in _cookieContainer.GetCookies(uri))
                        {
                            // Mostra nome e início do valor de cada cookie guardado
                            Console.WriteLine($"   Cookie: {cookie.Name} = {cookie.Value.Substring(0, Math.Min(cookie.Value.Length, 20))}...");
                        }
                    }
                    return loginResponse;
                }
                else
                {
                    // Erro HTTP: retorna um LoginResponse indicando falha e mostra a mensagem de erro
                    return new LoginResponse
                    {
                        Success = false,
                        Message = $"Erro HTTP {response.StatusCode}: {responseBody}"
                    };
                }
            }
            catch (Exception ex)
            {
                // Captura e retorna qualquer exceção como falha no login
                Console.WriteLine($"EXCECAO no login: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Erro de exceção: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Asynchronously logs out the current user by sending a POST request to the logout endpoint.
        /// </summary>
        /// <remarks>This method sends a POST request to the configured logout URL. If the request is
        /// successful, it returns an <see cref="ApiResponse"/> indicating success. If the request fails or an exception
        /// occurs, it returns an <see cref="ApiResponse"/> with a success status of <see langword="false"/> and an
        /// appropriate error message.</remarks>
        /// <returns>An <see cref="ApiResponse"/> object containing the result of the logout operation. The <c>Success</c>
        /// property is <see langword="true"/> if the logout was successful; otherwise, <see langword="false"/>.</returns>
        public async Task<ApiResponse> LogoutAsync()
        {
            try
            {
                // Monta a URL do endpoint de logout
                var logoutUrl = $"{_baseUrl}/api/auth/logout";
                Console.WriteLine($"DEBUG: Fazendo logout: {logoutUrl}");

                // Envia a requisição POST para o logout
                var response = await _httpClient.PostAsync(logoutUrl, null);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Logout Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    // Se for sucesso, tenta deserializar para ApiResponse
                    var logoutResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody, _jsonSettings);
                    return logoutResponse;
                }

                // Retorna objeto de resposta indicando falha
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Erro no logout: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                // Qualquer exceção, retorna resposta de erro
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Logs the user out by expiring all cookies associated with the current session.
        /// </summary>
        /// <remarks>This method marks all cookies for the current base URL as expired, effectively
        /// logging the user out from the local session. It does not communicate with the server to invalidate the
        /// session on the server side.</remarks>
        public void Logout()
        {
            var uri = new Uri(_baseUrl);
            foreach (Cookie cookie in _cookieContainer.GetCookies(uri))
            {
                cookie.Expired = true;  // Marca todos cookies dessa URL como expirados (faz logout local)
            }
            Console.WriteLine("Cookies locais removidos");
        }
        #endregion

        #region Usuário
        /// <summary>
        /// Asynchronously creates a new user in the database by sending a POST request to the API.
        /// </summary>
        /// <remarks>This method serializes the provided user object to JSON and sends it to the API
        /// endpoint for user creation. If the API call is successful, the response is deserialized to obtain the
        /// created user object.</remarks>
        /// <param name="user">The <see cref="UserDatabase"/> object representing the user to be created. Cannot be null.</param>
        /// <returns>The created <see cref="UserDatabase"/> object as returned by the API.</returns>
        /// <exception cref="Exception">Thrown if there is an error during the request or if the API returns an unsuccessful status code.</exception>
        public async Task<UserDatabase> CreateUserAsync(UserDatabase user)
        {
            try
            {
                // Serializa o objeto user para JSON
                var json = JsonConvert.SerializeObject(user, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Endpoint para criar usuário novo na API
                var usersUrl = $"{_baseUrl}/api/users";
                // Envia POST para a rota '/api/users' com JSON no corpo
                var response = await _httpClient.PostAsync(usersUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Criando usuário: {json}");
                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    // Se sucesso, deserializa resposta para UserResponse e retorna o usuário criado.
                    var apiResponse = JsonConvert.DeserializeObject<UserResponse>(responseBody, _jsonSettings);
                    return apiResponse.Data;
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");

                throw new Exception($"Erro ao criar usuário: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of user profiles from the access profiles API.
        /// </summary>
        /// <remarks>This method sends a GET request to the configured API endpoint to fetch user
        /// profiles. The response is deserialized into a list of <see cref="PerfilUsuario"/> objects.</remarks>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see
        /// cref="PerfilUsuario"/> objects representing the user profiles.</returns>
        public async Task<List<UserProfile>> GetPerfisAcessoAsync()
        {
            // Consulta endpoint de perfis de acesso
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/perfis");
            var json = await response.Content.ReadAsStringAsync();
            // Desserializa resultado para objeto de resposta
            var perfis = JsonConvert.DeserializeObject<ProfilesResponse>(json);
            return perfis.Data;
        }

        /// <summary>
        /// Asynchronously retrieves a list of users from the remote API.
        /// </summary>
        /// <remarks>This method sends a GET request to the users endpoint of the configured base URL. It
        /// deserializes the response into a list of <see cref="UserDatabase"/> objects if the request is
        /// successful.</remarks>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see
        /// cref="UserDatabase"/> objects.</returns>
        /// <exception cref="Exception">Thrown if there is an error in the request or if the response cannot be deserialized.</exception>
        public async Task<List<UserDatabase>> GetUsersAsync()
        {
            try
            {
                var usersUrl = $"{_baseUrl}/api/users";
                Console.WriteLine($"DEBUG: Buscando usuários: {usersUrl}");

                var response = await _httpClient.GetAsync(usersUrl);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Users Status: {response.StatusCode}");
                Console.WriteLine($"DEBUG: Resposta users (primeiros 300 chars): {responseBody.Substring(0, Math.Min(responseBody.Length, 300))}...");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Desserializa resposta para lista de usuários
                        var apiResponse = JsonConvert.DeserializeObject<UsersResponse>(responseBody, _jsonSettings);
                        if (apiResponse?.Data != null)
                        {
                            Console.WriteLine($"DEBUG: Deserializado {apiResponse.Data.Count} usuários do banco");
                            return apiResponse.Data;
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"DEBUG: Falha ao deserializar users: {ex.Message}");
                    }
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                }

                throw new Exception($"Erro ao buscar usuários: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously updates the user information for the specified user ID.
        /// </summary>
        /// <remarks>This method sends a PUT request to the server to update the user details. Ensure that
        /// the <paramref name="user"/> object contains valid data before calling this method.</remarks>
        /// <param name="id">The unique identifier of the user to be updated.</param>
        /// <param name="user">The <see cref="UserDatabase"/> object containing the updated user information.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the success or failure of the update operation, along with any
        /// response message from the server.</returns>
        public async Task<ApiResponse> UpdateUserAsync(int id, UserDatabase user)
        {
            // Monta a URL do usuário a ser atualizado pelo id
            var url = $"{_baseUrl}/api/users/{id}";
            var json = JsonConvert.SerializeObject(user, _jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return new ApiResponse
            {
                // Retorna se foi sucesso ou não
                Success = response.IsSuccessStatusCode,
                Message = responseBody
            };
        }

        // /
        public async Task<bool> DeleteUserAsync(int idUsuario, string motivo)
        {
            try
            {
                // Constrói URL de deletar e prepara corpo com motivo
                var url = $"{_baseUrl}/api/users/{idUsuario}";
                Console.WriteLine($"DEBUG: Deletando usuário: {url}");

                var body = new
                {
                    motivo = motivo
                };
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Cria request manual (HttpClient não tem DeleteAsync(url, content))
                var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);

                Console.WriteLine($"DeleteUserAsync Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição (deletar usuário): {ex.Message}");
            }
        }

        /// <summary>
        /// Represents the response received after requesting the deletion of users.
        /// </summary>
        /// <remarks>This response contains a list of backups for the users that were deleted.</remarks>
        public class DeletedUsersResponse : ApiResponse
        {
            public List<DeletedUserBackup> Data { get; set; }
        }

        /// <summary>
        /// Asynchronously retrieves a list of deleted users from the server.
        /// </summary>
        /// <remarks>This method sends a GET request to the server to fetch deleted user data. If the
        /// request is successful, it returns a list of <see cref="DeletedUserBackup"/> objects. If the server responds
        /// with an unauthorized status, an <see cref="UnauthorizedAccessException"/> is thrown. Any other unsuccessful
        /// response results in a generic exception being thrown.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
        /// cref="DeletedUserBackup"/> objects representing the deleted users. If no users are deleted, the list will be
        /// empty.</returns>
        /// <exception cref="Exception">Thrown if the request fails for any other reason, including network issues or server errors.</exception>
        public async Task<List<DeletedUserBackup>> GetDeletedUsersAsync()
        {
            try
            {
                var url = $"{_baseUrl}/api/users/deleted";
                Console.WriteLine($"DEBUG: Buscando usuários deletados: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"GetDeletedUsersAsync Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<DeletedUsersResponse>(responseBody);

                    if (apiResponse?.Data != null)
                        return apiResponse.Data;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                throw new Exception($"Erro ao buscar usuários deletados: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição (obter usuários deletados): {ex.Message}");
            }
        }

        /// <summary>
        /// Restores a user from a specified backup asynchronously.
        /// </summary>
        /// <param name="backupId">The identifier of the backup from which to restore the user. Must be a valid backup ID.</param>
        /// <returns><see langword="true"/> if the user is successfully restored; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the restore request.</exception>
        public async Task<bool> RestoreUserAsync(int backupId)
        {
            try
            {
                var url = $"{_baseUrl}/api/users/restore/{backupId}";
                Console.WriteLine($"DEBUG: Restaurando usuário do backup: {url}");

                // Envia POST sem body para o endpoint de restauração
                var response = await _httpClient.PostAsync(url, null); // Sem body necessário nesse caso

                Console.WriteLine($"RestoreUserAsync Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição (restaurar usuário): {ex.Message}");
            }
        }
        #endregion

        #region Chamados
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            var apiResponse = await GetApiAsync<TicketsResponse>("/api/chamados");

            if (apiResponse?.Data == null)
                return new List<Ticket>();

            Console.WriteLine($"DEBUG: Deserializado {apiResponse.Data.Count} chamados do banco");

            var tickets = apiResponse.Data.Select(db => new Ticket(db)).ToList();

            Console.WriteLine($"DEBUG: Primeiro chamado - ID: {tickets.FirstOrDefault()?.Id}, Titulo: {tickets.FirstOrDefault()?.Title}");

            return tickets;
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            Console.WriteLine($"DEBUG: Buscando chamado por ID: {id}");

            var apiResponse = await GetApiAsync<TicketResponse>($"/api/chamados/{id}");

            if (apiResponse?.Data == null)
                throw new Exception("Chamado não encontrado ou resposta inválida da API.");

            Console.WriteLine($"DEBUG: Chamado by ID response desserializado com sucesso.");

            return new Ticket(apiResponse.Data);
        }

        public async Task<Ticket> CreateTicketAsync(CreateTicketRequest ticket)
        {
            var apiResponse = await PostApiAsync<CreateTicketRequest, TicketResponse>("/api/chamados", ticket);

            if (apiResponse?.Data == null)
                throw new Exception("Resposta da API não contém dados do chamado criado.");

            var createdTicket = new Ticket(apiResponse.Data);
            Console.WriteLine($"DEBUG: Chamado criado - ID: {createdTicket.Id}, Titulo: {createdTicket.Title}");

            return createdTicket;
        }

        #region Aprovação e rejeição de chamados

        // Buscar chamados pendentes de aprovação
        public async Task<List<Ticket>> GetPendingTickets()
        {
            try
            {
                var url = $"{_baseUrl}/api/chamados/aprovacao";
                Console.WriteLine($"DEBUG: Buscando chamados para aprovação: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<TicketsResponse>(responseBody);
                    if (apiResponse?.Data != null)
                    {
                        var chamados = apiResponse.Data.Select(c => new Ticket(c)).ToList();
                        return chamados;
                    }
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                throw new Exception($"Erro ao buscar chamados: {response.StatusCode}");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        // Aprovar chamado
        public async Task<bool> ApproveTicketAsync(int idChamado)
        {
            try
            {
                var url = $"{_baseUrl}/api/chamados/{idChamado}/aprovar";
                Console.WriteLine($"DEBUG: Aprovando chamado: {url}");

                var response = await _httpClient.PostAsync(url, null);

                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        // Rejeitar chamado
        public async Task<bool> RejectTicketAsync(int idChamado, string motivo)
        {
            try
            {
                var url = $"{_baseUrl}/api/chamados/{idChamado}/rejeitar";
                Console.WriteLine($"DEBUG: Rejeitando chamado: {url}");

                var body = new { motivo = motivo };
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida.");
                }

                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        #endregion

        #region Escalonamento de Chamados

        public async Task ScaleTicketAsync(int idChamado, string motivo)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { motivo }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/chamados/{idChamado}/escalar", content);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao escalar chamado: {msg}");
            }
        }

        public async Task<List<ScaledTicket>> GetScaledTicketsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/chamados/escalados");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ScaledTicketsResponse>(json);
            return result?.Data ?? new List<ScaledTicket>();
        }
        #endregion

        #region Resolução de Chamados

        // Resolve chamados comuns
        public async Task ResolveTicketAsync(int idChamado, string motivoResolucao)
        {
            var body = new
            {
                id_chamado = idChamado,
                relatorio_resposta = motivoResolucao
            };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/chamados/resolver-com-relatorio", content);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao resolver chamado: {msg}");
            }
        }

        // Resolve chamados escalados
        public async Task ResolveScaledTicketAsync(int idChamado, string motivoResolucao)
        {
            var body = new
            {
                id_chamado = idChamado,
                relatorio_resposta = motivoResolucao
            };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/chamados/{idChamado}/resolver-escalado", content);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao resolver chamado escalado: {msg}");
            }
        }
        #endregion

        #endregion

        #region SoluçãoIA
        public async Task<IaResults> GetSolucaoIaAsync(int idChamado)
        {
            var url = $"{_baseUrl}/api/chamados/{idChamado}/solucao-ia";
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar solução IA: {response.StatusCode} - {responseBody}");

            var apiResponse = JsonConvert.DeserializeObject<IaResultsResponse>(responseBody, _jsonSettings);
            return apiResponse?.Data;
        }

        public async Task EnviarFeedbackIaAsync(int idChamado, string feedback)
        {
            var url = $"{_baseUrl}/api/chamados/{idChamado}/feedback-ia";
            var body = new { feedback }; // "DEU_CERTO" ou "DEU_ERRADO"
            var json = JsonConvert.SerializeObject(body, _jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao enviar feedback IA: {response.StatusCode} - {responseBody}");
        }
        #endregion

        #region Dashboard

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            try
            {
                // Monta URL de estatísticas
                var url = $"{_baseUrl}/api/estatisticas/dashboard-stats";
                Console.WriteLine($"DEBUG: Buscando dashboard stats: {url}");
                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<DashboardStatsResponse>(responseBody, _jsonSettings);
                    return apiResponse?.data ?? new DashboardStats();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                throw new Exception($"Erro ao buscar stats: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception($"Erro na requisição: {ex.Message}"); }
        }

        public async Task<List<MonthlyDataItem>> GetMonthlyDataAsync()
        {
            try
            {
                // Busca chamados mensais
                var url = $"{_baseUrl}/api/estatisticas/chamados-mensais";
                Console.WriteLine($"DEBUG: Buscando dados mensais: {url}");
                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<MonthlyDataResponse>(responseBody, _jsonSettings);
                    return apiResponse?.data ?? new List<MonthlyDataItem>();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                throw new Exception($"Erro ao buscar dados mensais: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception($"Erro na requisição: {ex.Message}"); }
        }

        public async Task<List<CategoryDataItem>> GetCategoryDataAsync()
        {
            try
            {
                // Busca chamados por categoria
                var url = $"{_baseUrl}/api/estatisticas/chamados-categoria";
                Console.WriteLine($"DEBUG: Buscando categorias: {url}");
                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<CategoryDataResponse>(responseBody, _jsonSettings);
                    return apiResponse?.data ?? new List<CategoryDataItem>();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                throw new Exception($"Erro ao buscar categorias: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception($"Erro na requisição: {ex.Message}"); }
        }

        public async Task<List<YearlyDataItem>> GetYearlyDataAsync()
        {
            try
            {
                // Busca chamados anuais
                var url = $"{_baseUrl}/api/estatisticas/chamados-anuais";
                Console.WriteLine($"DEBUG: Buscando dados anuais: {url}");
                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<YearlyDataResponse>(responseBody, _jsonSettings);
                    return apiResponse?.data ?? new List<YearlyDataItem>();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                throw new Exception($"Erro ao buscar dados anuais: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception($"Erro na requisição: {ex.Message}"); }
        }

        public async Task<List<AnalystDataItem>> GetAnalystDataAsync()
        {
            try
            {
                // Busca chamados com analistas
                var url = $"{_baseUrl}/api/estatisticas/chamados-analistas";
                Console.WriteLine($"DEBUG: Buscando chamados por analista: {url}");
                var response = await _httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<AnalystDataResponse>(responseBody, _jsonSettings);
                    return apiResponse?.data ?? new List<AnalystDataItem>();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Sessão expirada ou inválida. Faça login novamente.");
                throw new Exception($"Erro ao buscar analistas: {response.StatusCode} - {responseBody}");
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception($"Erro na requisição: {ex.Message}"); }
        }

        #endregion

        #region Debug
        public bool IsAuthenticated()
        {
            var uri = new Uri(_baseUrl);
            var cookies = _cookieContainer.GetCookies(uri);
            return cookies.Count > 0 && !cookies.Cast<Cookie>().All(c => c.Expired);
        }

        public void ShowSessionInfo()
        {
            Console.WriteLine("=== INFO DA SESSÃO ===");
            var uri = new Uri(_baseUrl);
            var cookies = _cookieContainer.GetCookies(uri);

            if (cookies.Count > 0)
            {
                Console.WriteLine($"Cookies ativos: {cookies.Count}");
                foreach (Cookie cookie in cookies)
                {
                    var status = cookie.Expired ? "EXPIRADO" : "ATIVO";
                    Console.WriteLine($"  • {cookie.Name}: {cookie.Value.Substring(0, Math.Min(cookie.Value.Length, 15))}... [{status}]");
                    Console.WriteLine($"    Domínio: {cookie.Domain} | Caminho: {cookie.Path}");
                }
            }
            else
            {
                Console.WriteLine("Nenhuma sessão ativa");
            }
            Console.WriteLine("========================");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
        #endregion
    }
}