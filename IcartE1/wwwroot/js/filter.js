var btn = document.getElementById("filterBtn");
var productList = document.getElementById("productList");
var branchList = document.getElementById("branchList");
var warehouseList = document.getElementById("warehouselist");

btn.addEventListener("click", () => {
    var productId = productList.value;
    var warehouseId = warehouseList.value;
    var branchId = branchList.value;
    location.href = location.host + "/branches?productId=" + productId + "&branchId=" + branchId + "&warehouseId=" + warehouseId;
});