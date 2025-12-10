using Microsoft.Win32;
using sistecDesktopRefactored.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models
{
    public class FileDialog : IFileDialogService
    {
        public string OpenImageFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Selecione a foto de perfil"
            };

            var result = dialog.ShowDialog();

            if (result == true)
                return dialog.FileName;

            return null;
        }
    }
}
