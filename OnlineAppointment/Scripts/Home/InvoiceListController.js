(function () {
    app.controller('InvoiceList', function ($scope, $http, $filter) {
        $scope.Sale = new Object();
        $scope.list = true;
        $scope.invoice = false;
        var init = function () {
            GetAllSales();
        }; //end of init
        init(); //init is called

        function GetAllSales() {
            $http.get('/Billing/GetAllSales')
                .then(function (response) {
                    var data = response.data;
                    $scope.SalesList = data;
                });
        }


        $scope.ShowHideEdit = function () {
            $scope.list = $scope.list == true ? false : true;
            $scope.invoice = $scope.invoice == true ? false : true;
        }

        $scope.GetInvoiceSalesBySalesId = function (salesId) {
            /*      GetProducts();*/
            $http.get('/Billing/GetInvoiceBySalesId', { params: { salesId: salesId } })
                .then(function (response) {
                    var data = response.data;
                    $scope.OrderList = data;
                    $scope.Sale.OrderNumber = $scope.OrderList[0].OrderNumber; //dagdag
                    $scope.Sale.Total = $scope.OrderList[0].Total; //dagdag
                    $scope.Sale.UserID = $scope.OrderList[0].UserID;
   
                });
        }

      
    });
}).call(angular);

/*<script src="~/Scripts/Home/app.js"></script>*/