using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {

        private readonly AppDbContext _context;
        public TodosController(AppDbContext context)
        {
            _context = context;
        }


        // [HttpGet]
        // public string SayHi()
        // {
        //     return "Hello Trial";
        // }

    [HttpGet] // read
    public async Task<IEnumerable<Todo>> GetTodo()
    {
        var todos = await _context.Todos.AsNoTracking().ToListAsync();
        return todos;
    }



    [HttpPost] // create

    public async Task<IActionResult> Create (Todo todo)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _context.AddAsync(todo);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            return Ok();
        }
        return BadRequest();
    }

    ///Delete
    [HttpDelete("{id:int}")]

    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }
        _context.Todos.Remove(todo);

        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            return Ok("Todo was deleted");
        }
        return BadRequest("Unable to delete Todo");
    }

  // Get a single Todo {id
  [HttpGet("{id:int}")]

  public async Task<ActionResult<Todo>> GetTodo(int id)
  {
    var todo = await _context.Todos.FindAsync(id);
    if(todo == null)
    {
        return NotFound("Sorry Todo not found");
    }
    return Ok(todo);
  }

    // Update PUT

    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditTodo(int id, Todo todo)
    {
       var todoFromDb = await _context.Todos.FindAsync(id);
       if( todoFromDb == null)
       {
        return BadRequest("Todo not found");
       }
        todoFromDb.Title = todo.Title;
        todoFromDb.Description = todo.Description;
        todoFromDb.Complete = todo.Complete;

        var result = await _context.SaveChangesAsync();
        if(result > 0)
        {
            return Ok("Todo was edited");
        }
        return BadRequest("Unable to update todo");

    }

    }
}