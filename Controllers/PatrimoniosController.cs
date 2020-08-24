using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using PatrimoniosV2.Model;


namespace PatrimoniosV2.Controllers
{
    [Route("api/empresa/[controller]")]
    [ApiController]
    public class PatrimoniosController : ControllerBase
    {
        private IConfiguration _config;

        public PatrimoniosController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // GET api/empresa/Patrimonios
        [HttpGet]
        public IEnumerable<Patrimonio> GetPatrimonios()
        {
            string getAll = "SELECT * FROM Patrimonio ORDER BY Nome";

            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                return conexao.Query<Patrimonio>(getAll);
            }
            
        }

        // GET api/empresa/Patrimonios/1
        [HttpGet("{id}")]
        public Patrimonio GetDetalhePatrimonio(int id)
        {
            string getPatrimonio = @"SELECT * FROM Patrimonio
                                    WHERE PatrimonioId = @PatrimonioId";
            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                return conexao.QueryFirst<Patrimonio>(getPatrimonio, new { PatrimonioId = id });
            }
        }

        // POST api/empresa/Patrimonio
        [HttpPost]
        public void Post([FromBody] Patrimonio patrimonio)
        {
            Random random = new Random();
            string postMarca = @"INSERT INTO Patrimonio (MarcaId, Nome, Descricao, NumeroTombo)
                                VALUES (@marcaId, @nome, @descricao, @tombo);";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(postMarca,
                    new { 
                        marcaId = patrimonio.MarcaId,
                        nome = patrimonio.Nome,
                        descricao = patrimonio.Descricao,
                        tombo = random.Next(1, 99999) + (patrimonio.MarcaId*100000)
                    },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }

        // PUT api/empresa/Patrimonio/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Patrimonio patrimonio)
        {
            string putMarca = @"UPDATE Patrimonio
                                    SET MarcaId = @marcaId,
                                        Nome = @nome,
                                        Descricao = @descricao
                                    WHERE PatrimonioId = @patrimonioId;";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(putMarca,
                    new
                    {
                        marcaId = patrimonio.MarcaId,
                        nome = patrimonio.Nome,
                        descricao = patrimonio.Descricao,
                        patrimonioId = id
                    },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }

        // DELETE api/empresa/Patrimonio/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteMarca = @"DELETE FROM Patrimonio
                                    WHERE PatrimonioId = @patrimonioId;";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(deleteMarca,
                    new { patrimonioId = id },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }
    }
}
