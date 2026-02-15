using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Globalization;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly RetailDbContext _context;

    public PricingController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCSV(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = null,
            PrepareHeaderForMatch = args => args.Header.ToLower()
        };

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<PricingRecord>().ToList();

        // Set CreatedDate if null
        foreach (var record in records)
        {
            record.CreatedDate ??= DateTime.Now;
        }

        await _context.PricingRecords.AddRangeAsync(records);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "File uploaded successfully",
            count = records.Count
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string? storeId, string? sku)
    {
        var query = _context.PricingRecords.AsQueryable();

        if (!string.IsNullOrEmpty(storeId))
            query = query.Where(x => x.StoreId == storeId);

        if (!string.IsNullOrEmpty(sku))
            query = query.Where(x => x.SKU == sku);

        var result = await query.ToListAsync();

        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PricingRecord model)
    {
        var record = await _context.PricingRecords.FindAsync(id);

        if (record == null)
            return NotFound();

        record.Price = model.Price;
        record.ProductName = model.ProductName;

        await _context.SaveChangesAsync();

        return Ok(record);
    }

}