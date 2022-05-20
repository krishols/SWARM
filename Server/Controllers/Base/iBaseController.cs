using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using SWARM.EF.Models;
using System.Threading.Tasks;
using SWARM.Shared;
using SWARM.Shared.DTO;

namespace SWARM.Server.Controllers.Base
{
    public interface iBaseController<T>
    {
        Task<IActionResult> Delete(string KeyValue);
        Task<IActionResult> Get();
        Task<IActionResult> Get(string KeyValue);
        Task<IActionResult> Post([FromBody] T _Item);
        Task<IActionResult> Put([FromBody] T _Item);
    }
}