angular.module("umbraco").controller("CodeMonkey.SeperatorController", function ($scope) {

	function formatDisplayValue() {

		if($scope.model.config.colorText == undefined || $scope.model.config.colorText === "") {
        	$scope.colorText = "#343434";
	    } else {
	        $scope.colorText = $scope.model.config.colorText;
	    }

	    if($scope.model.config.colorBackground == undefined || $scope.model.config.colorBackground === "") {
        	$scope.colorBackground = "#F8F8F8";
	    } else {
	        $scope.colorBackground = $scope.model.config.colorBackground;
	    }

		if($scope.model.label !== null && $scope.model.label !== "") {
			$scope.displayvalue = $scope.model.label;
		}
		else if($scope.model.config.seperatorText !== null && $scope.model.config.seperatorText !== "") {
        	$scope.displayvalue = $scope.model.config.seperatorText;
	    }
	    else {
	        $scope.displayvalue = "";
	    }
       	//console.log($scope.model.label);
    }

    //format the display value on init:
    formatDisplayValue();
});