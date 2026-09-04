package com.lowkeysdrops.web.controller;

import jakarta.servlet.http.HttpSession;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ModelAttribute;

@ControllerAdvice
public class GlobalControllerAdvice {

    @ModelAttribute("userRole")
    public String getUserRole(HttpSession session) {
        if (session != null && session.getAttribute("USER_ROLE") != null) {
            return (String) session.getAttribute("USER_ROLE");
        }
        return null;
    }

    @ModelAttribute("userName")
    public String getUserName(HttpSession session) {
        if (session != null && session.getAttribute("USER_NAME") != null) {
            return (String) session.getAttribute("USER_NAME");
        }
        return null;
    }
}
