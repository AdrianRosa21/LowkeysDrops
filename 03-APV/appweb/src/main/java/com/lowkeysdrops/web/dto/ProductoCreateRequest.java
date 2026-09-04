package com.lowkeysdrops.web.dto;
import java.math.BigDecimal;

public class ProductoCreateRequest {
    private Integer idDrop;
    private Integer idCategoria;
    private String nombre;
    private String descripcion;
    private String talla;
    private BigDecimal precio;
    private String imagenUrl;
    private Boolean esUnico;
    private Integer stock;

    // Getters and Setters
    public Integer getIdDrop() { return idDrop; }
    public void setIdDrop(Integer idDrop) { this.idDrop = idDrop; }
    public Integer getIdCategoria() { return idCategoria; }
    public void setIdCategoria(Integer idCategoria) { this.idCategoria = idCategoria; }
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
}
