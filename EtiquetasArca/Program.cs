namespace EtiquetasArca
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using Mutex singleInstanceMutex = new(true, @"Local\EtiquetasArca.SingleInstance", out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show(
                    "La aplicacion ya se esta ejecutando.",
                    "Arca de la Frontera - Etiquetas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            try
            {
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new Etiquetas());
            }
            finally
            {
                singleInstanceMutex.ReleaseMutex();
            }
        }
    }
}
