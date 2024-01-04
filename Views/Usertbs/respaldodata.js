
/*
 @model carnetutelvt.Models.Usertb

@{
    ViewData["Title"] = "DETALLES DE USUARIO";
}

<div class="hero">
    <center> <h1>Perfil usuario</h1></center>
    <div class="container">

        <form class="form">

    <dl class="row">
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Email)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Email)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Passwords)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Passwords)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Dateupdate)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Dateupdate)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Datecreate)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Datecreate)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Numberverify)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Numberverify)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Verifyuser)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Verifyuser)
        </dd>
         <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Rol)
         </dt>
         <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Rol)
         </dd>
         <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Typess)
         </dt>
          <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Typess)
          </dd>
    </dl>
        </form>

<div>
 <a type="submit" class="btn btn-success" asp-action="Edit" asp-route-id="@Model?.Id">Edit</a> |
 <a type="submit" class="btn btn-dark" asp-action="Index">Back</a>
</div>
</div>
</div>


if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {
                if (id == null || _context.Usertbs == null)
                {
                    return NotFound();
                }

                var usertb = await _context.Usertbs
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (usertb == null)
                {
                    return NotFound();
                }

                return View(usertb);
            }

*/