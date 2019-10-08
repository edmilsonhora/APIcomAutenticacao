using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:54801/api/");
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> form = new Dictionary<string, string>
                                              {
                                                  {"grant_type", "password"},
                                                  {"username", "joao"},
                                                  {"password", "123456"}
                                              };

            HttpResponseMessage r = client.PostAsync("token", new FormUrlEncodedContent(form)).Result;
            var retorno = JsonConvert.DeserializeObject<Token>(r.Content.ReadAsStringAsync().Result);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",retorno.access_token);
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            HttpResponseMessage r = client.GetAsync("Default").Result;
            var retorno = r.Content.ReadAsStringAsync().Result;
        }
    }
}
