package com.lowkeysdrops.web.dto;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import java.math.BigDecimal;

@JsonIgnoreProperties(ignoreUnknown = true)
public class ProductoResponse {
    private Integer idProducto;
    private String nombre;
    private String descripcion;
    private String talla;
    private BigDecimal precio;
    private String imagenUrl;
    private Boolean esUnico;
    private Integer stock;
    private String estado;
    
    // We get Category and Drop details directly from API occasionally or just ID
    private Integer idCategoria;
    private Integer idDrop;

    // Getters and Setters
    public Integer getIdProducto() { return idProducto; }
    public void setIdProducto(Integer idProducto) { this.idProducto = idProducto; }
    public String getNombre() { return nombre; }
    public void setNombre(String nombre) { this.nombre = nombre; }
    public String getDescripcion() { return descripcion; }
    public void setDescripcion(String descripcion) { this.descripcion = descripcion; }
    public String getTalla() { return talla; }
    public void setTalla(String talla) { this.talla = talla; }
    public BigDecimal getPrecio() { return precio; }
    public void setPrecio(BigDecimal precio) { this.precio = precio; }
    public String getImagenUrl() { return imagenUrl; }
    public void setImagenUrl(String imagenUrl) { this.imagenUrl = imagenUrl; }
    public Boolean getEsUnico() { return esUnico; }
    public void setEsUnico(Boolean esUnico) { this.esUnico = esUnico; }
    public Integer getStock() { return stock; }
    public void setStock(Integer stock) { this.stock = stock; }
    public String getEstado() { return estado; }
    public void setEstado(String estado) { this.estado = estado; }
    public Integer getIdCategoria() { return idCategoria; }
    public void setIdCategoria(Integer idCategoria) { this.idCategoria = idCategoria; }
    public Integer getIdDrop() { return idDrop; }
    public void setIdDrop(Integer idDrop) { this.idDrop = idDrop; }
}
