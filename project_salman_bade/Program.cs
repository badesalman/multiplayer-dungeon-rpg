using System;//konsol işlemleri
using System.Text;

namespace ConOOD_RpgGame // prjisim
{
    internal class Program // başlatma classı
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // oyun başlatma 
            Console.CursorVisible = false;

            Game game = new Game();
            game.Run();
        }
    }
}