var app = angular.module('ReasonMasterModule', []);

app.service('ReasonMasterService', function ($http, $location) {


    this.GetReasonMasterList = function () {

        return $http.get('api/ReasonMaster/GetReasonMaster', {})

    };


    this.DeleteReason = function (Id, EmployeeCode) {

        return $http.get('api/ReasonMaster/DeleteReason?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {})

    };

    this.SaveReason = function (list) {

        return $http.post('api/ReasonMaster/SaveReason', list, {})

    };


});



app.controller('ReasonMasterController', function ($scope, ReasonMasterService, $rootScope) {
    console.log("ReasonMaster");

    $scope.ReasonMasterList = [];


    $scope.init = function () {
        ReasonMasterService.GetReasonMasterList().then(function (success) {

            if (success.data != null) {
                $scope.ReasonMasterList = success.data;

            }

        }, function (error) {

            console.log(error);

        });

    };

    $scope.Update = function () {
        console.log($scope.ReasonMasterList);

        angular.forEach($scope.ReasonMasterList, function (value, key) {
            value.EmployeeCode = $rootScope.session.EMP_CODE;
        });

        ReasonMasterService.SaveReason($scope.ReasonMasterList).then(function (success) {

            swal("Success", success.data, "success");
            $scope.init();

        }, function (error) {

            console.log(error);

        });

    };

    $scope.AddNewRow = function () {
        $scope.ReasonMasterList.push({
            Reason: ""
        });


        console.log($scope.ReasonMasterList);
    };


    $scope.RemoveRow = function (Id, index) {


        if (Id) {
            swal({
                title: "Are you sure?",
                text: "You want to delete.",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
function () {

    ReasonMasterService.DeleteReason(Id, $rootScope.session.EMP_CODE).then(function (success) {
        $scope.init();
        swal("Deleted!", success.data, "success");
    }, function (error) {

        console.log(error);

    });
});

        }
        else {
            $scope.ReasonMasterList.splice(index, 1);
        }

        console.log($scope.ReasonMasterList);
    };

    $scope.init();
});
