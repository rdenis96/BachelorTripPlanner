globalModule.directive('interestsSlider', function ($timeout, $window) {
    return {
        restrict: 'E',
        scope: {
            interests: '='
        },
        link: function (scope, elem, attrs) {

            var transportsIcons = {
                Bus: 'Bus',
                Metro: 'Metro',
                Train: 'Train',
                Boat: 'Boat',
                Tram: 'Tram',
                Taxi: 'Taxi',
                Underground: 'Underground',
                Subway: 'Subway',
                Helicopter: 'Helicopter',
                Plane: 'Plane',
                Rent: 'Rent'
            }

            scope.getTransportIcon = function (transport) {
                transport = transport.trim();
                switch (transport) {
                    case transportsIcons.Bus:
                        return "Images/Common/BusIcon.png";
                    case transportsIcons.Metro:
                        return "Images/Common/MetroIcon.png";
                    case transportsIcons.Train:
                        return "Images/Common/TrainIcon.png";
                    case transportsIcons.Boat:
                        return "Images/Common/BoatIcon.png";
                    case transportsIcons.Tram:
                        return "Images/Common/TramIcon.png";
                    case transportsIcons.Taxi:
                        return "Images/Common/TaxiIcon.png";
                    case transportsIcons.Underground:
                        return "Images/Common/UndergroundIcon.png";
                    case transportsIcons.Subway:
                        return "Images/Common/SubwayIcon.png";
                    case transportsIcons.Helicopter:
                        return "Images/Common/HelicopterIcon.png";
                    case transportsIcons.Plane:
                        return "Images/Common/PlaneIcon.png";
                    case transportsIcons.Rent:
                        return "Images/Common/RentIcon.png";
                }
            }

            scope.currentIndex = 0; // Initially the index is at the first interest

            scope.next = function () {
                scope.currentIndex < scope.interests.length - 1 ? scope.currentIndex++ : scope.currentIndex = 0;
            };

            scope.prev = function () {
                scope.currentIndex > 0 ? scope.currentIndex-- : scope.currentIndex = scope.interests.length - 1;
            };

            scope.openWikipediaTab = function (link) {
                $window.open(link, '_blank');
            };

            scope.$watch('currentIndex', function () {
                scope.interests.forEach(function (interest) {
                    interest.visible = false; // make every interest invisible
                });

                scope.interests[scope.currentIndex].visible = true; // make the current interest visible
            });

            var timer;
            var sliderFunc = function () {
                timer = $timeout(function () {
                    scope.next();
                    timer = $timeout(sliderFunc, 4000);
                }, 4000);
            };

            sliderFunc();

            scope.$on('$destroy', function () {
                $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
            });
        },
        templateUrl: 'AppViews/Home/home-suggested-interests-slidetemplate.html'
    };
});