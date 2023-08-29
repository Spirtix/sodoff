﻿using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ItemStoreController : Controller {

    private readonly DBContext ctx;
    private StoreService storeService;
    public ItemStoreController(DBContext ctx, StoreService storeService) {
        this.ctx = ctx;
        this.storeService = storeService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetStore")]
    public IActionResult GetStore([FromForm] string getStoreRequest) {
        GetStoreRequest request = XmlUtil.DeserializeXml<GetStoreRequest>(getStoreRequest);

        ItemsInStoreData[] stores = new ItemsInStoreData[request.StoreIDs.Length];
        for (int i = 0; i < request.StoreIDs.Length; i++) {
            stores[i] = storeService.GetStore(request.StoreIDs[i]);
        }

        GetStoreResponse response = new GetStoreResponse {
            Stores = stores
        };

        return Ok(response);
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetRankAttributeData")]
    public IActionResult GetRankAttributeData() {
        // TODO
        return Ok(XmlUtil.ReadResourceXmlString("rankattrib"));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetAnnouncementsByUser")]
    //[VikingSession(UseLock=false)]
    public IActionResult GetAnnouncements([FromForm] int worldObjectID) {
        // TODO: This is a placeholder, although this endpoint seems to be only used to send announcements to the user (such as the server shutdown), so this might be sufficient.
        return Ok(new AnnouncementList());
    }
}
