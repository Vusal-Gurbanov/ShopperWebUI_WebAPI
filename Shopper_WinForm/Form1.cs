using Shopper_WebAPI.Identity;

namespace Shopper_WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btn_get_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5049/");
            HttpResponseMessage response = await client.GetAsync("api/User");
            string result = await response.Content.ReadAsStringAsync();
            List<User> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(result);
            lstUsers.DataSource = users;
        }
    }
}