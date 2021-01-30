globalModule.directive('interestsSlider', function ($timeout, $window) {
    return {
        restrict: 'E',
        scope: {
            interests: '='
        },
        link: function (scope, elem, attrs) {
            var weatherIcons = {
                Rainy: 'Rainy',
                Stormy: 'Stormy',
                Sunny: 'Sunny',
                Warm: 'Warm',
                Cloudy: 'Cloudy',
                Hot: 'Hot',
                Cold: 'Cold',
                Dry: 'Dry',
                Wet: 'Wet',
                Windy: 'Windy',
                Hurricanes: 'Hurricanes',
                Typhoons: 'Typhoons',
                SandStorms: 'SandStorms',
                SnowStorms: 'SnowStorms',
                Tornados: 'Tornados',
                Humid: 'Humidity',
                Foggy: 'Foggy',
                Snow: 'Snow',
                Thundersnow: 'Thundersnow',
                Hail: 'Hail',
                Sleet: 'Sleet',
                Drought: 'Drought',
                Wildfire: 'Wildfire',
                Blizzard: 'Blizzard',
                Avalanche: 'Avalanche',
                Mist: 'Mist',
                Mild: 'Mild',
                Gloomy: 'Gloomy',
            };

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
            };

            scope.getWeatherIcon = function (weather) {
                weather = weather.trim();
                switch (weather) {
                    case weatherIcons.Rainy:
                        return "Images/Common/weather-rainy.png";
                    case weatherIcons.Stormy:
                        return "Images/Common/weather-stormy.png";
                    case weatherIcons.Sunny:
                        return "Images/Common/weather-sunny.png";
                    case weatherIcons.Warm:
                        return "Images/Common/weather-warm.png";
                    case weatherIcons.Cloudy:
                        return "Images/Common/weather-cloudy.png";
                    case weatherIcons.Hot:
                        return "Images/Common/weather-hot.png";
                    case weatherIcons.Cold:
                        return "Images/Common/weather-cold.png";
                    case weatherIcons.Dry:
                        return "Images/Common/weather-dry.png";
                    case weatherIcons.Wet:
                        return "Images/Common/weather-wet.png";
                    case weatherIcons.Windy:
                        return "Images/Common/weather-windy.png";
                    case weatherIcons.Hurricanes:
                        return "Images/Common/weather-hurricanes.png";
                    case weatherIcons.Typhoons:
                        return "Images/Common/weather-typhoons.png";
                    case weatherIcons.SandStorms:
                        return "Images/Common/weather-sandstorm.png";
                    case weatherIcons.SnowStorms:
                        return "Images/Common/weather-snowstorm.png";
                    case weatherIcons.Tornados:
                        return "Images/Common/weather-tornado.png";
                    case weatherIcons.Humid:
                        return "Images/Common/weather-humid.png";
                    case weatherIcons.Foggy:
                        return "Images/Common/weather-foggy.png";
                    case weatherIcons.Snow:
                        return "Images/Common/weather-snow.png";
                    case weatherIcons.Thundersnow:
                        return "Images/Common/weather-thundersnow.png";
                    case weatherIcons.Hail:
                        return "Images/Common/weather-hail.png";
                    case weatherIcons.Sleet:
                        return "Images/Common/weather-sleet.png";
                    case weatherIcons.Drought:
                        return "Images/Common/weather-drought.png";
                    case weatherIcons.Wildfire:
                        return "Images/Common/weather-wildfire.png";
                    case weatherIcons.Blizzard:
                        return "Images/Common/weather-blizzard.png";
                    case weatherIcons.Avalanche:
                        return "Images/Common/weather-avalanche.png";
                    case weatherIcons.Mist:
                        return "Images/Common/weather-mist.png";
                    case weatherIcons.Mild:
                        return "Images/Common/weather-mild.png";
                    case weatherIcons.Gloomy:
                        return "Images/Common/weather-gloomy.png";
                    default:
                        return "Images/Common/weather-unknown.png";
                }
            };

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
                    default:
                        return "Images/Common/Vehicle.png";
                }
            };

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

                if (scope.interests.length > 0) {
                    scope.interests[scope.currentIndex].visible = true; // make the current interest visible
                }
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