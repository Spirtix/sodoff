using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Model;

namespace sodoff.Controllers.Common;
public class ItemStoreController : Controller {

    private readonly DBContext ctx;
    public ItemStoreController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetStore")]
    public IActionResult GetStore() {
        // TODO, this may be implemented enough, but may not be
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("store.xml"));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream)) {
            string result = reader.ReadToEnd();
            return Ok(result);
        }
    }
}
