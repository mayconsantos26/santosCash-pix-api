// using DTOs;
// using Microsoft.AspNetCore.Mvc;
// using Services;

// namespace MicroTeste.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class TransacoesController : ControllerBase
//     {
//         private readonly ItransacoesServices _transacoesServices;

//         // Injeção de dependência do serviço de transações
//         public TransacoesController(ItransacoesServices transacoesServices)
//         {
//             _transacoesServices = transacoesServices;
//         }

//         // GET: api/Transacoes
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<TransacoesDTO>>> GetAllTransacoes()
//         {
//             try
//             {
//                 var transacoes = await _transacoesServices.GetAll();
//                 return Ok(transacoes); // Retorna 200 OK com a lista de transações
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { message = ex.Message }); // Retorna erro 400 em caso de falha
//             }
//         }

//         // GET: api/Transacoes/{txid}
//         [HttpGet("{txid}")]
//         public async Task<ActionResult<TransacoesDTO>> GetTransacaoByTxid(string txid)
//         {
//             try
//             {
//                 var transacao = await _transacoesServices.GetTransacoesDTOByTxidAsync(txid);
//                 if (transacao == null)
//                 {
//                     return NotFound(new { message = "Transação não encontrada" }); // Retorna 404 se não encontrar
//                 }
//                 return Ok(transacao); // Retorna 200 OK com a transação encontrada
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { message = ex.Message }); // Retorna erro 400 em caso de falha
//             }
//         }

//         // POST: api/Transacoes
//         // [HttpPost]
//         // public async Task<ActionResult<TransacoesCreateDTO>> CreateTransacao(TransacoesCreateDTO transacoesCreateDTO)
//         // {
//         //     try
//         //     {
//         //         // Chama o serviço para criar a transação
//         //         var createdTransacao = await _transacoesServices.CreateTransacoesDTOAsync(transacoesCreateDTO);

//         //         // Retorna o DTO com a transação criada, incluindo o e2e_id e o Id gerados
//         //         return CreatedAtAction(nameof(GetTransacaoById), new { id = createdTransacao.Id }, createdTransacao);
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         return BadRequest(new { message = ex.Message }); // Retorna erro 400 em caso de falha
//         //     }   
//         // }

//         // PUT: api/Transacoes/{id}
//         // [HttpPut("{txid}")]
//         // public async Task<ActionResult<TransacoesDTO>> UpdateTransacao(string txid, TransacoesDTO transacoesDTO)
//         // {
//         //     try
//         //     {
//         //         if (txid != transacoesDTO.Txid)
//         //         {
//         //             return BadRequest(new { message = "O ID da URL não corresponde ao ID do corpo da requisição." });
//         //         }

//         //         var updatedTransacao = await _transacoesServices.UpdateTransacoesDTOAsync(transacoesDTO);
//         //         return Ok(updatedTransacao); // Retorna 200 OK com a transação atualizada
//         //     }
//         //     catch (KeyNotFoundException)
//         //     {
//         //         return NotFound(new { message = "Transação não encontrada." }); // Retorna erro 404 se não encontrar a transação
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         return BadRequest(new { message = ex.Message }); // Retorna erro 400 em caso de falha
//         //     }
//         // }

//         // DELETE: api/Transacoes/{id}
//         [HttpDelete("{txid}")]
//         public async Task<ActionResult<TransacoesDTO>> DeleteTransacao(string txid)
//         {
//             try
//             {
//                 var deletedTransacao = await _transacoesServices.DeleteTransacoesDTOAsync(txid);
//                 if (deletedTransacao == null)
//                 {
//                     return NotFound(new { message = "Transação não encontrada." }); // Retorna erro 404 se não encontrar
//                 }
//                 return Ok(deletedTransacao); // Retorna 200 OK com a transação deletada
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { message = ex.Message }); // Retorna erro 400 em caso de falha
//             }
//         }
//     }
// }

// // Explicação do Controller:
// // Injeção de Dependência: O controller recebe uma instância do ItransacoesServices via injeção de dependência, permitindo que ele delegue a lógica de negócios para o serviço.

// // Endpoints:

// // [HttpGet]: Este método retorna todas as transações. Ele chama o método GetAll do serviço e retorna um 200 OK com a lista de transações.
// // [HttpGet("{id}")]: Este método retorna uma transação específica pelo ID. Ele chama GetTransacoesDTOByIdAsync do serviço e retorna um 200 OK se a transação for encontrada ou um 404 Not Found se não for.
// // [HttpPost]: Este método cria uma nova transação. Ele chama o método CreateTransacoesDTOAsync do serviço e retorna um 201 Created com o recurso criado, incluindo o ID da transação criada.
// // [HttpPut("{id}")]: Este método atualiza uma transação existente. Ele chama UpdateTransacoesDTOAsync do serviço e retorna um 200 OK com a transação atualizada ou um 404 Not Found se não encontrar a transação.
// // [HttpDelete("{id}")]: Este método deleta uma transação. Ele chama DeleteTransacoesDTOAsync do serviço e retorna um 200 OK com a transação deletada ou um 404 Not Found se não encontrar a transação.
// // Tratamento de Erros: Para cada operação, há um tratamento básico de erros:

// // BadRequest: Quando há erros de validação ou problemas nos dados enviados.
// // NotFound: Quando o recurso (transação) não é encontrado.
// // CreatedAtAction: Quando uma nova transação é criada, o código retorna um 201 Created com a URL da nova transação.
// // Como Funciona:
// // Ao chamar POST /api/Transacoes, você cria uma nova transação.
// // Ao chamar GET /api/Transacoes, você obtém todas as transações.
// // Ao chamar GET /api/Transacoes/{id}, você obtém uma transação específica.
// // Ao chamar PUT /api/Transacoes/{id}, você atualiza uma transação existente.
// // Ao chamar DELETE /api/Transacoes/{id}, você deleta uma transação.
