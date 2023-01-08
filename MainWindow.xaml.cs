using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Runtime.InteropServices;

namespace PixelColorBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwflags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void OnButtonSearchPixelClick(object sender, RoutedEventArgs e)
        {
            string inputHexColorCode = TextBoxHexColor.Text;
            SearchPixel(inputHexColorCode);
        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            Click();
            System.Threading.Thread.Sleep(250);
            Click();
        }
        private bool SearchPixel(string hexCode)
        {
            //Ekran boyutu ile aynı boyutta bir bitmap oluşturur.
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //Ekranın görüntüsünü almak için Graphics objesi oluşturur.
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            //Ekrandaki içeriği graphics objesine dönüştürür.
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            // Hex'i (Örn: #ffffff) Color objesine dönüştürür.
            Color desiredPixelColor = ColorTranslator.FromHtml(hexCode);

            for (int x = 0; x < SystemInformation.VirtualScreen.Width; x++)
            {
                for (int y = 0; y < SystemInformation.VirtualScreen.Height; y++)
                {
                    // Mevcut pixelin rengini almak için
                    Color currentPixelColor = bitmap.GetPixel(x, y);

                    // pixellerdeki hex kodu ile istediğimiz hex kodunu karşılaştırmak için
                    if (desiredPixelColor == currentPixelColor)
                    {
                        MessageBox.Show(String.Format("Found Pixel at {0}, {1} - Now set mouse cursor", x, y));
                        DoubleClickAtPosition(x, y);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
