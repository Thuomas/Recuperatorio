using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SegundoParcial.Data;
using SegundoParcial.Models;
using SegundoParcial.Services;
using SegundoParcial.ViewModels;

namespace SegundoParcial.Controllers;

    public class DepositoController : Controller
    {
        private IAreaService _areaService;
        private IDepositoService _depositoService;

        public DepositoController(IAreaService areaService, IDepositoService depositoService)
        {
            _areaService = areaService;
            _depositoService = depositoService;
        }

        // GET: Deposito
        public async Task<IActionResult> Index(string nameFilter)
        {
            var list = _depositoService.GetAll(nameFilter);
            var model = new DepositoViewModel();
            model.Depositos = list;
            var areaList = _areaService.GetAll();
            ViewData["Areas"] = new SelectList(areaList, "Id", "Nombre");
            return View(model);
        }

        // GET: Deposito/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposito = _depositoService.GetById(id.Value);
            if (deposito == null)
            {
                return NotFound();
            }

            var model = new DepositoViewModel();
            model.Areas = deposito.Areas;
            model.Nombre = deposito.Nombre;
            model.Id = deposito.Id;

            return View(model);
        }

        // GET: Deposito/Create
        public IActionResult Create()
        {
            var areaList = _areaService.GetAll();
            ViewData["Areas"] = new SelectList(areaList, "Id", "Nombre");
            return View();
        }

        // POST: Deposito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,AreaIds")] DepositoCreateViewModel depositoView)
        {

            if (ModelState.IsValid)
            {
                var areas = _areaService.GetAll().Where(x => depositoView.AreaIds.Contains(x.Id)).ToList();
                var deposito = new Deposito
                {
                    Nombre = depositoView.Nombre,
                    Areas = areas
                };

                _depositoService.Create(deposito);

                return RedirectToAction(nameof(Index));
            }
            return View(depositoView);
        }



        // GET: Deposito/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposito = _depositoService.GetById(id.Value);
            if (deposito == null)
            {
                return NotFound();
            }
            DepositoCreateViewModel depositoN = new DepositoCreateViewModel();
            depositoN.Id = deposito.Id;
            depositoN.Nombre = deposito.Nombre;

            var areaList = _areaService.GetAll();
            ViewData["Areas"] = new SelectList(areaList, "Id", "Nombre");

            return View(depositoN);
        }

        // POST: Deposito/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,AreaIds")] DepositoCreateViewModel depositoView)
        {
            if (id != depositoView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var areas = _areaService.GetAll().Where(x => depositoView.AreaIds.Contains(x.Id)).ToList();
                var depositoN = _depositoService.GetById(depositoView.Id);
                depositoN.Nombre = depositoView.Nombre;
                depositoN.Areas = areas;
                _depositoService.Update(depositoN);

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Deposito/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposito = _depositoService.GetById(id.Value);
            if (deposito == null)
            {
                return NotFound();
            }
            DepositoViewModel depositoN = new DepositoViewModel();
            depositoN.Id = deposito.Id;

            return View(depositoN);
        }

        // POST: Deposito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deposito = _depositoService.GetById(id);

            if (deposito != null)
            {
                _depositoService.Delete(deposito.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepositoExists(int id)
        {
            return _depositoService.GetById(id) != null;
        }
    }

