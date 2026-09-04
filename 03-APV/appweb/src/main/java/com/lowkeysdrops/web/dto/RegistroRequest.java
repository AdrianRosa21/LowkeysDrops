package com.lowkeysdrops.web.dto;

public class RegistroRequest {
    private String nombre;
    private String correo;
    private String contrasena;
    private String telefono;
    private String dui;

    // Getters and Setters
    public String getNombre() { return nombre; }
    public void setNombre(String nombre) { this.nombre = nombre; }
    public String getCorreo() { return correo; }
    public void setCorreo(String correo) { this.correo = correo; }
    public String getContrasena() { return contrasena; }
    public void setContrasena(String contrasena) { this.contrasena = contrasena; }
    public String getTelefono() { return telefono; }
    public void setTelefono(String telefono) { this.telefono = telefono; }
    public String getDui() { return dui; }
    public void setDui(String dui) { this.dui = dui; }
}
