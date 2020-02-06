using System.Windows.Forms;
using E_mailing;

namespace Emailing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Email email = new Email();
            email.FromAddress = "Ahmad.Mkashati@syriatel.net";
            email.ToAddress = "Ahmad.Mkashati@syriatel.net";
            email.CCAddress = "Ahmad.Mkashati@syriatel.net";
            email.Subject = "TEST MAIL";
            email.Body = "TEST MAIL" + "<br>" + "Regards";
            email.Host = "email.syriatel.com";
            email.Send();
        }
    }
}
