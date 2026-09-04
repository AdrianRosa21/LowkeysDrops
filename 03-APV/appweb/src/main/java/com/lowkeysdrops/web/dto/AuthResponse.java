package com.lowkeysdrops.web.dto;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@JsonIgnoreProperties(ignoreUnknown = true)
public class AuthResponse {
    private String token;
    private UsuarioResponse usuario;

    public String getToken() { return token; }
    public void setToken(String token) { this.token = token; }
    public UsuarioResponse getUsuario() { return usuario; }
    public void setUsuario(UsuarioResponse usuario) { this.usuario = usuario; }
}
