using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoApiContext _context;

        public TodoController(TodoApiContext context)
        {
            _context = context;

            if (!_context.TodoItem.Any())
            {
                _context.TodoItem.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }


        // GET api/todo
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItem.ToList();
        }

        // GET api/todo/{id}
        [HttpGet("{id}", Name = "GetTodoList")]
        public ActionResult<TodoItem> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var item = _context.TodoItem.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/todo
        [HttpPost]
        public IActionResult Post([FromBody] TodoItem item)
        {
            _context.TodoItem.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetTodoList", new { id = item.Id }, item);
        }

        // PUT api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TodoItem item)
        {
            if (id == 0 || item == null)
            {
                return BadRequest();
            }

            var todo = _context.TodoItem.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItem.Update(todo);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.TodoItem.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
