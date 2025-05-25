using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURANDO EF CORE COM ORACLE
builder.Services.AddDbContext<RfidTrackingApi.RfidContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RfidTracking API", Version = "v1" });
});

var app = builder.Build();

// MIDDLEWARES
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RfidTracking API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace RfidTrackingApi
{
    // DTOs
    public class MotoDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Required]
        public string Modelo { get; set; } = string.Empty;

        public string Status { get; set; } = "Disponível";

        public int? LeitorRFIDId { get; set; }
    }

    public class LeitorDto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Localizacao { get; set; } = string.Empty;
    }

    public class RegistroDto
    {
        public int Id { get; set; }

        [Required]
        public int MotoId { get; set; }

        [Required]
        public int LeitorRFIDId { get; set; }
    }

    // MODELOS
    public class Moto
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Status { get; set; } = "Disponível";
        public int? LeitorRFIDId { get; set; }
        public LeitorRFID? LeitorRFID { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.UtcNow;
    }

    public class LeitorRFID
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();
    }

    public class RegistroLeituraRFID
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public Moto? Moto { get; set; }
        public int LeitorRFIDId { get; set; }
        public LeitorRFID? LeitorRFID { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }

    // CONTEXTO EF CORE
    public class RfidContext : DbContext
    {
        public RfidContext(DbContextOptions<RfidContext> options) : base(options) { }

        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<LeitorRFID> Leitores => Set<LeitorRFID>();
        public DbSet<RegistroLeituraRFID> Registros => Set<RegistroLeituraRFID>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Moto>().ToTable("MOTOS");
            modelBuilder.Entity<LeitorRFID>().ToTable("LEITORES");
            modelBuilder.Entity<RegistroLeituraRFID>().ToTable("REGISTROS");
        }
    }

    // CONTROLLERS
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly RfidContext _context;

        public MotosController(RfidContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _context.Motos.Include(m => m.LeitorRFID).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.Include(m => m.LeitorRFID).FirstOrDefaultAsync(m => m.Id == id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<Moto>> PostMoto([FromBody] MotoDto dto)
        {
            var moto = new Moto
            {
                Placa = dto.Placa,
                Modelo = dto.Modelo,
                Status = dto.Status,
                LeitorRFIDId = dto.LeitorRFIDId
            };
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoto(int id, [FromBody] MotoDto dto)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            moto.Placa = dto.Placa;
            moto.Modelo = dto.Modelo;
            moto.Status = dto.Status;
            moto.LeitorRFIDId = dto.LeitorRFIDId;
            moto.DataUltimaAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class LeitoresController : ControllerBase
    {
        private readonly RfidContext _context;

        public LeitoresController(RfidContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeitorRFID>>> GetLeitores()
        {
            return Ok(await _context.Leitores.Include(l => l.Motos).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeitorRFID>> GetLeitor(int id)
        {
            var leitor = await _context.Leitores.Include(l => l.Motos).FirstOrDefaultAsync(l => l.Id == id);
            if (leitor == null) return NotFound();
            return Ok(leitor);
        }

        [HttpPost]
        public async Task<ActionResult<LeitorRFID>> PostLeitor([FromBody] LeitorDto dto)
        {
            var leitor = new LeitorRFID { Nome = dto.Nome, Localizacao = dto.Localizacao };
            _context.Leitores.Add(leitor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLeitor), new { id = leitor.Id }, leitor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeitor(int id, [FromBody] LeitorDto dto)
        {
            var leitor = await _context.Leitores.FindAsync(id);
            if (leitor == null) return NotFound();
            leitor.Nome = dto.Nome;
            leitor.Localizacao = dto.Localizacao;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeitor(int id)
        {
            var leitor = await _context.Leitores.FindAsync(id);
            if (leitor == null) return NotFound();
            _context.Leitores.Remove(leitor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class RegistrosController : ControllerBase
    {
        private readonly RfidContext _context;

        public RegistrosController(RfidContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroLeituraRFID>>> GetRegistros()
        {
            return Ok(await _context.Registros.Include(r => r.Moto).Include(r => r.LeitorRFID).OrderByDescending(r => r.DataHora).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroLeituraRFID>> GetRegistro(int id)
        {
            var registro = await _context.Registros.Include(r => r.Moto).Include(r => r.LeitorRFID).FirstOrDefaultAsync(r => r.Id == id);
            if (registro == null) return NotFound();
            return Ok(registro);
        }

        [HttpPost]
        public async Task<ActionResult<RegistroLeituraRFID>> PostRegistro([FromBody] RegistroDto dto)
        {
            var registro = new RegistroLeituraRFID
            {
                MotoId = dto.MotoId,
                LeitorRFIDId = dto.LeitorRFIDId,
                DataHora = DateTime.UtcNow
            };
            _context.Registros.Add(registro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRegistro), new { id = registro.Id }, registro);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.Registros.FindAsync(id);
            if (registro == null) return NotFound();
            _context.Registros.Remove(registro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}