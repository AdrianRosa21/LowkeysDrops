package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.ProductoCreateRequest;
import com.lowkeysdrops.web.dto.ProductoUpdateRequest;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.CategoriaService;
import com.lowkeysdrops.web.service.DropService;
import com.lowkeysdrops.web.service.ProductoService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/admin/productos")
public class ProductoController {

    private final ProductoService productoService;
    private final CategoriaService categoriaService;
    private final DropService dropService;

    public ProductoController(ProductoService productoService, CategoriaService categoriaService, DropService dropService) {
        this.productoService = productoService;
        this.categoriaService = categoriaService;
        this.dropService = dropService;
    }

    @GetMapping
    public String index(Model model) {
        try {
            model.addAttribute("productos", productoService.getAll());
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar productos: " + e.getMessage());
        }
        return "productos/index";
    }

    @GetMapping("/nuevo")
    public String createForm(Model model) {
        model.addAttribute("producto", new ProductoCreateRequest());
        try {
            model.addAttribute("categorias", categoriaService.getAll());
            model.addAttribute("drops", dropService.getAll());
        } catch (ApiException ignored) {}
        return "productos/form_create";
    }

    @PostMapping("/nuevo")
    public String create(@ModelAttribute ProductoCreateRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            productoService.create(request);
            redirectAttributes.addFlashAttribute("success", "Producto creado correctamente.");
            return "redirect:/admin/productos";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al crear: " + e.getMessage());
            model.addAttribute("producto", request);
            try {
                model.addAttribute("categorias", categoriaService.getAll());
                model.addAttribute("drops", dropService.getAll());
            } catch (ApiException ignored) {}
            return "productos/form_create";
        }
    }

    @GetMapping("/{id}/editar")
    public String editForm(@PathVariable Integer id, Model model) {
        try {
            var p = productoService.getById(id);
            ProductoUpdateRequest req = new ProductoUpdateRequest();
            req.setNombre(p.getNombre());
            req.setDescripcion(p.getDescripcion());
            req.setTalla(p.getTalla());
            req.setPrecio(p.getPrecio());
            req.setImagenUrl(p.getImagenUrl());
            req.setEsUnico(p.getEsUnico());
            req.setStock(p.getStock());
            req.setEstado(p.getEstado());
            model.addAttribute("producto", req);
            model.addAttribute("idProducto", id);
            return "productos/form_update";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar producto: " + e.getMessage());
            return "productos/index";
        }
    }

    @PostMapping("/{id}/editar")
    public String edit(@PathVariable Integer id, @ModelAttribute ProductoUpdateRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            productoService.update(id, request);
            redirectAttributes.addFlashAttribute("success", "Producto actualizado correctamente.");
            return "redirect:/admin/productos";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al actualizar: " + e.getMessage());
            model.addAttribute("producto", request);
            model.addAttribute("idProducto", id);
            return "productos/form_update";
        }
    }

    @PostMapping("/{id}/eliminar")
    public String delete(@PathVariable Integer id, RedirectAttributes redirectAttributes) {
        try {
            productoService.delete(id);
            redirectAttributes.addFlashAttribute("success", "Producto eliminado correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error al eliminar: " + e.getMessage());
        }
        return "redirect:/admin/productos";
    }
}
