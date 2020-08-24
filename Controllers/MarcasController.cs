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
    public class MarcasController : ControllerBase
    {
        private IConfiguration _config;

        public MarcasController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // GET api/empresa/marcas
        [HttpGet]
        public IEnumerable<Marca> GetMarcas()
        {
            string getAll = "SELECT * FROM Marca ORDER BY Nome";

            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                return connection.Query<Marca>(getAll);
            }            
        }

        // GET api/empresa/marcas/1
        [HttpGet("{id}")]
        public ContentResult GetDetalheMarca(int id)
        {
            string responseJSON;
            string getMarca = @"SELECT
                                m.MarcaId AS 'Marca.MarcaId',
                                m.Nome AS 'Marca.Nome',
                                p.PatrimonioId AS 'Marca.Patrimonios.PatrimonioId',
                                p.Nome AS 'Marca.Patrimonios.Nome'
                            FROM Marca AS m
                            INNER JOIN Patrimonio AS p ON m.MarcaId = p.MarcaId
                            WHERE m.MarcaId = @marcaId
                            FOR JSON PATH";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                responseJSON = connection.QueryFirst<string>(getMarca, new { marcaId = id });
            }

            return Content(responseJSON, "application/json");
        }

        // GET api/empresa/marca/1/patrimonios
        [HttpGet("{id}/patrimonios")]
        public IEnumerable<Patrimonio> GetPatrimoniosFromMarca(int id)
        {
            string getPatrimonioMarca = @"SELECT p.*,
                                            m.Nome AS 'marcaNome'
                                            FROM Marca AS m
                                            INNER JOIN Patrimonio AS p ON m.MarcaId = p.MarcaId
                                            WHERE m.MarcaId = @marcaId";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                return connection.Query<Patrimonio>(getPatrimonioMarca, new { marcaId = id });
            }
        }

        // POST api/empresa/marca
        [HttpPost]
        public void Post([FromBody] Marca marca)
        {
            string postMarca = @"INSERT INTO Marca (Nome)
                                    VALUES (@marca);";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(postMarca, 
                    new { marca = marca.Nome },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }

        // PUT api/empresa/marca/2
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Marca marca)
        {
            string putMarca = @"UPDATE Marca
                                    SET Nome = @nome
                                    WHERE MarcaId = @marcaId;";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(putMarca,
                    new { nome = marca.Nome, marcaId = id },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }

        // DELETE api/empresa/marca/3
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteMarca = @"DELETE FROM Marca
                                    WHERE MarcaId = @marcaId;";
            using (SqlConnection connection = new SqlConnection(
                _config.GetConnectionString("Empresa")))
            {
                connection.Open();
                var transacao = connection.BeginTransaction();

                connection.Execute(deleteMarca,
                    new { marcaId = id },
                    transaction: transacao);

                transacao.Commit();
                connection.Close();
            }
        }
    }
}
