package com.lowkeysdrops.web.dto;

public class DropRequest {
    private String nombre;
    private String descripcion;
    private String fechaPublicacion;
    private String estado;

    public String getNombre() { return nombre; }
    public void setNombre(String nombre) { this.nombre = nombre; }
    public String getDescripcion() { return descripcion; }
    public void setDescripcion(String descripcion) { this.descripcion = descripcion; }
    public String getFechaPublicacion() { return fechaPublicacion; }
    public void setFechaPublicacion(String fechaPublicacion) { this.fechaPublicacion = fechaPublicacion; }
    public String getEstado() { return estado; }
    public void setEstado(String estado) { this.estado = estado; }
}
