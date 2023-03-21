using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebJITDebugger.Dtos;
using WebJITDebugger.Services;

namespace WebJITDebugger.Controllers;

[ApiController]
[Route("[controller]")]
public class DisassemblerController : ControllerBase
{
    private readonly ILogger<DisassemblerController> _logger;
    private readonly DisassemblerService _disassemblerService;

    public DisassemblerController(ILogger<DisassemblerController> logger, DisassemblerService disassemblerService)
    {
        _logger = logger;
        _disassemblerService = disassemblerService;
    }

    [HttpPost]
    public async Task<ActionResult<CodeResponse>> Disassembly(CodeRequest request)
    {
        var response = await _disassemblerService.DisassemblyCode(request.Code);
        
        return Ok(new CodeResponse()
        {
            AssemblyCode = response
        });
    }
}