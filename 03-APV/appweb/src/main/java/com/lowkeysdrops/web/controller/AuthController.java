package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.AuthResponse;
import com.lowkeysdrops.web.dto.LoginRequest;
import com.lowkeysdrops.web.dto.RegistroRequest;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.AuthService;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
public class AuthController {
    
    private final AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    @GetMapping("/login")
    public String loginPage(Model model) {
        model.addAttribute("loginRequest", new LoginRequest());
        return "auth/login";
    }

    @PostMapping("/login")
    public String login(@ModelAttribute LoginRequest loginRequest, HttpSession session, RedirectAttributes redirectAttributes, Model model) {
        try {
            AuthResponse response = authService.login(loginRequest);
            session.setAttribute("AUTH_TOKEN", response.getToken());
            session.setAttribute("AUTH_USER", response.getUsuario());
            session.setAttribute("USER_ID", response.getUsuario().getIdUsuario());
            session.setAttribute("USER_NAME", response.getUsuario().getNombre());
            session.setAttribute("USER_ROLE", response.getUsuario().getRol());
            
            String rol = response.getUsuario().getRol();
            if ("ADMIN".equals(rol)) {
                return "redirect:/admin";
            } else if ("REPARTIDOR".equals(rol)) {
                return "redirect:/repartidor";
            } else {
                return "redirect:/cliente";
            }
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
            return "auth/login";
        }
    }

    @GetMapping("/registro")
    public String registroPage(Model model) {
        model.addAttribute("registroRequest", new RegistroRequest());
        return "auth/registro";
    }

    @PostMapping("/registro")
    public String registro(@ModelAttribute RegistroRequest registroRequest, RedirectAttributes redirectAttributes, Model model) {
        try {
            authService.registrar(registroRequest);
            redirectAttributes.addFlashAttribute("success", "Cuenta creada correctamente. Ya puedes iniciar sesión.");
            return "redirect:/login";
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
            return "auth/registro";
        }
    }

    @GetMapping("/logout")
    public String logout(HttpSession session, RedirectAttributes redirectAttributes) {
        session.invalidate();
        return "redirect:/login";
    }
}
