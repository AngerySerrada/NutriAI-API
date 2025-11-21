-- Poblar Sexos
INSERT INTO Entidad.Sexos (Descripcion) VALUES
('Masculino'),
('Femenino'),
('Otro');

-- Poblar Niveles de Actividad Física
INSERT INTO Entidad.NivelesActividad (Descripcion) VALUES
('Sedentario'),
('Ligero'),
('Moderado'),
('Activo'),
('Muy activo');


-- Poblar Comunas de la Región Metropolitana
INSERT INTO Entidad.Comunas (Nombre) VALUES
('Alhué'),
('Buin'),
('Calera de Tango'),
('Cerrillos'),
('Cerro Navia'),
('Colina'),
('Conchalí'),
('Curacaví'),
('El Bosque'),
('El Monte'),
('Estación Central'),
('Huechuraba'),
('Independencia'),
('Isla de Maipo'),
('La Cisterna'),
('La Florida'),
('La Granja'),
('Lampa'),
('La Pintana'),
('La Reina'),
('Las Condes'),
('Lo Barnechea'),
('Lo Espejo'),
('Lo Prado'),
('Macul'),
('Maipú'),
('María Pinto'),
('Melipilla'),
('Ñuñoa'),
('Padre Hurtado'),
('Paine'),
('Pedro Aguirre Cerda'),
('Peñaflor'),
('Peñalolén'),
('Pirque'),
('Providencia'),
('Pudahuel'),
('Puente Alto'),
('Quilicura'),
('Quinta Normal'),
('Recoleta'),
('Renca'),
('San Bernardo'),
('San Joaquín'),
('San José de Maipo'),
('San Miguel'),
('San Pedro'),
('San Ramón'),
('Santiago'),
('Talagante'),
('Tiltil'),
('Vitacura');

INSERT INTO Maestro.Ingredientes (Nombre, Categoria, Calorias, Proteinas, Carbohidratos, Grasas) VALUES
('Arroz blanco', 'Cereales', 130, 2.4, 28.0, 0.3),
('Fideos', 'Cereales', 131, 5.0, 25.0, 1.1),
('Avena', 'Cereales', 389, 17.0, 66.0, 7.0),
('Pan marraqueta', 'Panadería', 265, 9.0, 49.0, 3.0),

('Papa', 'Verduras', 77, 2.0, 17.0, 0.1),
('Zanahoria', 'Verduras', 41, 0.9, 10.0, 0.2),
('Cebolla', 'Verduras', 40, 1.1, 9.3, 0.1),
('Tomate', 'Verduras', 18, 0.9, 3.9, 0.2),
('Ajo', 'Verduras', 149, 6.4, 33.0, 0.5),
('Pimiento', 'Verduras', 31, 1.0, 6.0, 0.3),
('Lechuga', 'Verduras', 15, 1.4, 2.9, 0.2),
('Espinaca', 'Verduras', 23, 2.9, 3.6, 0.4),

('Manzana', 'Frutas', 52, 0.3, 14.0, 0.2),
('Plátano', 'Frutas', 89, 1.1, 23.0, 0.3),
('Naranja', 'Frutas', 47, 0.9, 12.0, 0.1),
('Limón', 'Frutas', 29, 1.1, 9.3, 0.3),
('Pera', 'Frutas', 57, 0.4, 15.0, 0.1),
('Frutilla', 'Frutas', 32, 0.7, 7.7, 0.3),

('Pollo pechuga', 'Carnes', 165, 31.0, 0.0, 3.6),
('Carne molida', 'Carnes', 250, 26.0, 0.0, 15.0),
('Cerdo lomo', 'Carnes', 242, 27.0, 0.0, 14.0),
('Pescado merluza', 'Pescados', 85, 18.0, 0.0, 1.2),
('Atún en agua', 'Pescados', 116, 26.0, 0.0, 1.0),

('Huevo', 'Huevos', 155, 13.0, 1.1, 11.0),

('Leche entera', 'Lácteos', 61, 3.2, 5.0, 3.3),
('Queso fresco', 'Lácteos', 98, 11.0, 3.0, 4.3),
('Yogur natural', 'Lácteos', 59, 10.0, 3.6, 0.4),

('Aceite de oliva', 'Aceites', 884, 0, 0, 100),
('Aceite vegetal', 'Aceites', 884, 0, 0, 100),
('Mantequilla', 'Grasas', 717, 0.9, 0.1, 81.0),

('Porotos cocidos', 'Legumbres', 127, 8.7, 22.8, 0.5),
('Lentejas cocidas', 'Legumbres', 116, 9.0, 20.0, 0.4),
('Garbanzos cocidos', 'Legumbres', 164, 8.9, 27.4, 2.6),

('Azúcar', 'Otros', 387, 0, 100, 0),
('Harina', 'Otros', 364, 10.0, 76.0, 1.0),
('Sal', 'Otros', 0, 0, 0, 0),
('Miel', 'Otros', 304, 0.3, 82.0, 0.0),
('Chocolate amargo', 'Otros', 546, 4.9, 46.0, 31.0);

INSERT INTO Maestro.Ingredientes (Nombre, Categoria, Calorias, Proteinas, Carbohidratos, Grasas) VALUES
-- Verduras
('Brócoli', 'Verduras', 34, 2.8, 7.0, 0.4),
('Coliflor', 'Verduras', 25, 1.9, 5.0, 0.3),
('Zapallo italiano', 'Verduras', 17, 1.2, 3.1, 0.3),
('Pepino', 'Verduras', 16, 0.7, 3.6, 0.1),
('Repollo', 'Verduras', 25, 1.3, 6.0, 0.1),
('Betarraga', 'Verduras', 43, 1.6, 10.0, 0.2),
('Apio', 'Verduras', 16, 0.7, 3.0, 0.2),
('Champiñones', 'Verduras', 22, 3.1, 3.3, 0.3),
('Berenjena', 'Verduras', 25, 1.0, 6.0, 0.2),
('Palta', 'Verduras', 160, 2.0, 9.0, 15.0),

-- Frutas
('Kiwi', 'Frutas', 61, 1.1, 15.0, 0.5),
('Sandía', 'Frutas', 30, 0.6, 8.0, 0.2),
('Melón', 'Frutas', 34, 0.8, 8.0, 0.2),
('Piña', 'Frutas', 50, 0.5, 13.0, 0.1),
('Durazno', 'Frutas', 39, 0.9, 10.0, 0.2),
('Arándanos', 'Frutas', 57, 0.7, 14.0, 0.3),
('Uvas', 'Frutas', 69, 0.7, 18.0, 0.2),
('Mango', 'Frutas', 60, 0.8, 15.0, 0.4),

-- Carnes y pescados
('Carne de vacuno posta rosada', 'Carnes', 219, 26.0, 0.0, 12.0),
('Carne de vacuno asiento', 'Carnes', 250, 21.0, 0.0, 18.0),
('Pechuga de pavo', 'Carnes', 135, 29.0, 0.0, 1.0),
('Salchicha', 'Procesados', 295, 11.0, 3.0, 26.0),
('Jamón de pavo', 'Procesados', 109, 18.0, 2.0, 3.0),
('Salmon', 'Pescados', 208, 20.0, 0.0, 13.0),
('Jurel', 'Pescados', 158, 20.0, 0.0, 8.0),
('Sardinas', 'Pescados', 208, 25.0, 0.0, 11.0),

-- Lácteos
('Queso mantecoso', 'Lácteos', 356, 24.0, 3.0, 28.0),
('Queso cheddar', 'Lácteos', 403, 25.0, 1.3, 33.0),
('Leche descremada', 'Lácteos', 34, 3.4, 5.0, 0.1),
('Leche semidescremada', 'Lácteos', 50, 3.5, 5.0, 1.5),
('Quesillo', 'Lácteos', 98, 11.0, 3.0, 4.0),
('Kéfir natural', 'Lácteos', 60, 3.0, 7.0, 2.0),

-- Huevos y derivados
('Clara de huevo', 'Huevos', 52, 11.0, 0.7, 0.2),
('Huevo duro', 'Huevos', 155, 13.0, 1.1, 11.0),

-- Legumbres
('Porotos negros cocidos', 'Legumbres', 132, 8.9, 23.7, 0.5),
('Arvejas cocidas', 'Legumbres', 81, 5.4, 14.0, 0.4),
('Tofu', 'Legumbres', 76, 8.0, 1.9, 4.8),
('Soja cocida', 'Legumbres', 141, 12.0, 11.0, 6.0),

-- Cereales y derivados
('Cuscús', 'Cereales', 112, 3.8, 23.0, 0.2),
('Quinoa cocida', 'Cereales', 120, 4.4, 21.0, 1.9),
('Polenta', 'Cereales', 70, 1.5, 15.0, 0.4),
('Pan integral', 'Panadería', 247, 13.0, 41.0, 4.0),

-- Salsas / condimentos
('Mayonesa', 'Salsas', 680, 1.0, 0.0, 75.0),
('Ketchup', 'Salsas', 112, 1.0, 26.0, 0.3),
('Mostaza', 'Salsas', 66, 4.4, 5.0, 4.0),
('Salsa de soya', 'Salsas', 53, 8.0, 5.0, 0.6),
('Salsa BBQ', 'Salsas', 167, 1.0, 40.0, 0.3),

-- Semillas / frutos secos
('Almendras', 'Frutos secos', 579, 21.0, 22.0, 50.0),
('Nueces', 'Frutos secos', 654, 15.0, 14.0, 65.0),
('Maní', 'Frutos secos', 567, 26.0, 16.0, 49.0),
('Semillas de chia', 'Semillas', 486, 17.0, 42.0, 31.0),
('Semillas de linaza', 'Semillas', 533, 18.0, 29.0, 42.0),

-- Especias
('Orégano', 'Especias', 265, 9.0, 69.0, 4.0),
('Pimentón en polvo', 'Especias', 282, 14.0, 55.0, 13.0),
('Comino', 'Especias', 375, 18.0, 44.0, 22.0),
('Cúrcuma', 'Especias', 354, 8.0, 65.0, 10.0),
('Canela', 'Especias', 247, 4.0, 81.0, 1.2),

-- Otros
('Yogur griego', 'Lácteos', 59, 10.0, 3.6, 0.4),
('Gelatina sin sabor', 'Otros', 62, 16.0, 0.0, 0.0),
('Café instantáneo', 'Otros', 2, 0.1, 0.0, 0.0),
('Té negro', 'Otros', 1, 0.1, 0.0, 0.0);


INSERT INTO [Maestro].[Enfermedades] (Nombre, Descripcion)
VALUES ('Diabetes', 'Diabetes tipo 1 o 2'),
       ('Hipertensión', 'Presión arterial alta'),
       ('Asma', 'Asma crónica'),
       ('Alergia', 'Alergias varias');

INSERT INTO [Entidad].[UsuarioEnfermedad] 
    (IdUsuario, IdEnfermedad, FechaDiagnostico, Observaciones)
VALUES
    (1, 1, '2020-05-10', 'Controlado con medicamentos'),
    (1, 3, '2018-09-15', 'Ataques esporádicos');
