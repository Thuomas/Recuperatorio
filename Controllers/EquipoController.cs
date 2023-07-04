using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SegundoParcial.Data;
using SegundoParcial.Models;
using SegundoParcial.Services;
using SegundoParcial.ViewModels;

namespace SegundoParcial.Controllers;
[Authorize]
    public class EquipoController : Controller
    {
        private IEquipoService _equipoService;
        private IDepositoService _depositoService;

        public EquipoController(IEquipoService equipoService, IDepositoService depositoService )
        {

            _equipoService = equipoService;
            _depositoService = depositoService;
        }

        // GET: Equipo
        public async Task<IActionResult> Index(string nameFilter)
        {
            var list = _equipoService.GetAll(nameFilter);
            var model = new EquipoViewModel();
            model.Equipos = list;
            var depositoList = _depositoService.GetAll();
            ViewData["Depositos"] = new SelectList(depositoList, "Id", "Nombre");
            return View(model);
        }

        // GET: Equipo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var equipo = _equipoService.GetById(id.Value);
            if (equipo == null)
            {
                return NotFound();
            }
            
            var model= new EquipoViewModel();
            model.NumSerie = equipo.NumSerie;
            model.Id = equipo.Id;
            model.FechaProd = equipo.FechaProd;
            model.FechaVenta = equipo.FechaVenta;
            model.Comentario = equipo.Comentario;
            model.DepositoId = equipo.DepositoId;
            var depositoList = _depositoService.GetAll();
            ViewData["Depositos"] = new SelectList(depositoList, "Id", "Nombre");
            return View(model);
        }

        // GET: Equipo/Create
        [Authorize(Roles = "Stock")]
        public IActionResult Create()
        {
            var depositoList = _depositoService.GetAll();
            ViewData["Depositos"] = new SelectList(depositoList, "Id", "Nombre");
            return View();
        }

        // POST: Equipo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumSerie,FechaProd,Comentario,DepositoId")] EquipoCreateViewModel equipoView)
        {
            if (ModelState.IsValid)
            {
                var deposito = _depositoService.GetById(equipoView.DepositoId);
               var equipo = new Equipo{
                NumSerie = equipoView.NumSerie,
                FechaProd = equipoView.FechaProd,
                Comentario = equipoView.Comentario,
                DepositoId = deposito.Id                
               };
               _equipoService.Create(equipo);
               return RedirectToAction(nameof(Index));
            }
            
            return View(equipoView);
        }

        // GET: Equipo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var equipo = _equipoService.GetById(id.Value);
            if (equipo == null)
            {
                return NotFound();
            }
            
            EquipoEditViewModel equipoEdit = new EquipoEditViewModel();
            equipoEdit.Id = equipo.Id;
            equipoEdit.NumSerie = equipo.NumSerie;
            equipoEdit.FechaProd = equipo.FechaProd;
            equipoEdit.FechaVenta = equipo.FechaVenta;
            equipoEdit.Comentario = equipo.Comentario;

            var depositoList = _depositoService.GetAll();
            ViewData["Depositos"] = new SelectList(depositoList, "Id", "Nombre");
            return View(equipoEdit);
        }

        // POST: Equipo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumSerie,FechaProd,FechaVenta,Comentario,DepositoId")] EquipoEditViewModel equipoview)
        {
            if (id != equipoview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               var deposito = _depositoService.GetById(equipoview.DepositoId);
               var equipoAModificar= _equipoService.GetById(equipoview.Id);
               equipoAModificar.NumSerie = equipoview.NumSerie;
               equipoAModificar.FechaProd = equipoview.FechaProd;
               equipoAModificar.FechaVenta= equipoview.FechaVenta;
               equipoAModificar.Comentario = equipoview.Comentario;
               equipoAModificar.DepositoId = deposito.Id;

               _equipoService.Update(equipoAModificar);
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Equipo/Delete/5
        [Authorize(Roles = "Stock")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var equipo =_equipoService.GetById(id.Value);
            if (equipo == null)
            {
                return NotFound();
            }
            EquipoViewModel equipoN = new EquipoViewModel();
            equipoN.Id = equipo.Id;
            equipoN.NumSerie = equipo.NumSerie;
            equipoN.FechaProd = equipo.FechaProd;
            equipoN.FechaVenta = equipo.FechaVenta;
            equipoN.Comentario = equipo.Comentario;
            return View(equipoN);
        }

        // POST: Equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var equipo = _equipoService.GetById(id);

            if (equipo != null)
            {
                _equipoService.Delete(equipo.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(int id)
        {
          return _equipoService.GetById(id) != null;
        }
    }

