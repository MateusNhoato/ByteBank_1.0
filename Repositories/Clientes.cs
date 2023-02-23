using Entities;
using Newtonsoft.Json;

namespace Byte_Bank_1_0.Repositories
{
    public class Clientes
    {
        private string _path = @"..\..\..\Repositories\Dados\Clientes.json";

        public List<Cliente>? PuxarBancoDeDados()
        {
            List<Cliente> clientes = null;

            try
            {
                string json = File.ReadAllText(_path);
                clientes = JsonConvert.DeserializeObject<List<Cliente>>(json);
            }
            catch (FileNotFoundException e)
            {
                File.Create(_path).Close();
            }
            catch (DirectoryNotFoundException e)
            {
            }

            return clientes;
        }

        public void AtualizarListaDeClientes(List<Cliente> clientes)
        {
            string json = JsonConvert.SerializeObject(clientes);
            try
            {

                File.WriteAllText(_path, json);

            }
            catch (FileNotFoundException)
            {
                File.Create(_path).Close();
                File.WriteAllText(_path, json);
            }
            catch (DirectoryNotFoundException e)
            { }

        }


    }
}
