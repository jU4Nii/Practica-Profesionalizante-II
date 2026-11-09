(() => {
    const ingreso = document.getElementById("ingreso");
    const egreso = document.getElementById("egreso");
    const concepto = document.getElementById("Concepto");
    const tipoIngreso = document.getElementById("tipoIngreso");
    const detalleEgreso = document.getElementById("detalleEgreso");
    const campoIngreso = document.getElementById("campoIngreso");
    const campoEgreso = document.getElementById("campoEgreso");

    if (!ingreso || !egreso || !concepto || !tipoIngreso || !detalleEgreso || !campoIngreso || !campoEgreso) return;

    function actualizarMovimiento() {
        const esIngreso = ingreso.checked;
        campoIngreso.classList.toggle("d-none", !esIngreso);
        campoEgreso.classList.toggle("d-none", esIngreso);
        detalleEgreso.required = !esIngreso;
        concepto.value = esIngreso ? tipoIngreso.value : detalleEgreso.value;
    }

    ingreso.addEventListener("change", actualizarMovimiento);
    egreso.addEventListener("change", actualizarMovimiento);
    tipoIngreso.addEventListener("change", actualizarMovimiento);
    detalleEgreso.addEventListener("input", actualizarMovimiento);
    actualizarMovimiento();
})();
