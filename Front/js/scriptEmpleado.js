$(document).ready(function () {
  obtenerEmpleados();
  obtenerCargos();

  $("#empleados").on("click", "tr", function () {
      var idEmpleado = $(this).data("id");
      var empleadoSeleccionado = empleados.find(function (empleado) {
          return empleado.idEmpleado === idEmpleado;
      });

      $("#idEmpleado").val(empleadoSeleccionado.idEmpleado);
      $("#nombre").val(empleadoSeleccionado.nombre);
      $("#apellido").val(empleadoSeleccionado.apellido);
      $("#cargo").val(empleadoSeleccionado.idCargo);
      $("#edad").val(empleadoSeleccionado.edad);
      $("#salario").val(empleadoSeleccionado.salario);

      $("#idEmpleado").show();
    
  });

  $("#btnLimpiar").on("click", function () {
      limpiarFormulario();
      $("#idEmpleado").val("");
      $("#idEmpleado").hide();
      habilitarCampos(true);
  });

  $("#btnGuardar").on("click", function () {
      guardarEmpleado();
  });

  $("#btnEliminar").on("click", function () {
      var idEmpleado = $("#idEmpleado").val();
      if (idEmpleado) {
          eliminarEmpleado(idEmpleado);
      } else {
          Swal.fire({
              icon: "error",
              title: "No se ha seleccionado ningún empleado",
              text: "Por favor, selecciona un empleado de la lista antes de eliminar."
          });
      }
  });

  $("#btnActualizar").on("click", function () {
      actualizarEmpleado();
  });
});

var empleados = [];

function obtenerEmpleados() {
  $.ajax({
      url: "https://localhost:7234/getEmpleados",
      type: "GET",
      success: function (data) {
          empleados = data;
          actualizarTablaEmpleados(data);
      },
      error: function () {
          alert("Error al obtener los empleados.");
      }
  });
}

function obtenerCargos() {
  $.ajax({
      url: "https://localhost:7234/getCargos",
      type: "GET",
      success: function (cargos) {
          var options = cargos.map(function (cargo) {
              return '<option value="' + cargo.idCargo + '">' + cargo.descripcion + '</option>';
          });
          $("#cargo").append(options.join(""));
      },
      error: function () {
          alert("Error al obtener los cargos.");
      }
  });
}

function actualizarTablaEmpleados(data) {
  $("#empleados").empty();

  data.forEach(function (empleado) {
      var nombreCompleto = empleado.nombre + " " + empleado.apellido;
      var fila =
          '<tr data-id="' + empleado.idEmpleado + '">' +
          '<td>' + empleado.idEmpleado + '</td>' +
          '<td>' + nombreCompleto + '</td>' +
          '<td>' + empleado.cargo + '</td>' +
          '<td>' + empleado.edad + '</td>' +
          '<td>' + empleado.salario + '</td>' +
          '</tr>';
      $("#empleados").append(fila);
  });
}

function limpiarFormulario() {
  $("#nombre").val("");
  $("#apellido").val("");
  $("#edad").val("");
  $("#salario").val("");
}

function guardarEmpleado() {
  var nombre = $("#nombre").val();
  var apellido = $("#apellido").val();
  var cargo = $("#cargo").val();
  var edad = $("#edad").val();
  var salario = $("#salario").val();

  if (!nombre || !apellido || !cargo || !edad || !salario) {
      Swal.fire({
          icon: "error",
          title: "Campos incompletos",
          text: "Por favor, completa todos los campos antes de guardar el empleado."
      });
      return;
  }

  var nuevoEmpleado = {
      nombre: nombre,
      apellido: apellido,
      idCargo: cargo,
      edad: edad,
      salario: salario
  };

  $.ajax({
      url: "https://localhost:7234/postEmpleado",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(nuevoEmpleado),
      success: function () {
          Swal.fire({
              icon: "success",
              title: "Empleado guardado exitosamente",
              text: "El empleado ha sido guardado correctamente."
          });
          limpiarFormulario();
          obtenerEmpleados();
      },
      error: function () {
          Swal.fire({
              icon: "error",
              title: "Error al guardar el empleado",
              text: "Ocurrió un error al guardar el empleado. Por favor, inténtalo nuevamente."
          });
      }
  });
}

function eliminarEmpleado(idEmpleado) {
  $.ajax({
      url: "https://localhost:7234/deleteEmpleado/" + idEmpleado,
      type: "DELETE",
      success: function () {
          Swal.fire({
              icon: "success",
              title: "Empleado eliminado exitosamente",
              text: "El empleado ha sido eliminado correctamente."
          });
          limpiarFormulario();
          obtenerEmpleados();
          $("#idEmpleado").hide();
          habilitarCampos(true);
      },
      error: function () {
          Swal.fire({
              icon: "error",
              title: "Error al eliminar el empleado",
              text: "Ocurrió un error al eliminar el empleado. Por favor, inténtalo nuevamente."
          });
      }
  });
}


function actualizarEmpleado() {
  var idEmpleado = $("#idEmpleado").val();
  var nombre = $("#nombre").val();
  var apellido = $("#apellido").val();
  var cargo = $("#cargo").val();
  var edad = $("#edad").val();
  var salario = $("#salario").val();

  if (!idEmpleado || !nombre || !apellido || !cargo || !edad || !salario) {
      Swal.fire({
          icon: "error",
          title: "Campos incompletos",
          text: "Por favor, completa todos los campos antes de actualizar el empleado."
      });
      return;
  }

  var empleadoActualizado = {
      id: parseInt(idEmpleado),
      nombre: nombre,
      apellido: apellido,
      idCargo: parseInt(cargo),
      edad: parseInt(edad),
      salario: parseFloat(salario)
  };

  $.ajax({
      url: "https://localhost:7234/putEmpleado/" + idEmpleado,
      type: "PUT",
      contentType: "application/json",
      data: JSON.stringify(empleadoActualizado),
      success: function () {
          Swal.fire({
              icon: "success",
              title: "Empleado actualizado exitosamente",
              text: "El empleado ha sido actualizado correctamente."
          });
          limpiarFormulario();
          obtenerEmpleados();
          $("#idEmpleado").hide();
     
      },
      error: function () {
          Swal.fire({
              icon: "error",
              title: "Error al actualizar el empleado",
              text: "Ocurrió un error al actualizar el empleado. Por favor, inténtalo nuevamente."
          });
      }
  });
}
