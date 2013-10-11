
$(function() {
    var hub = $.connection.loginHub;
    $.connection.hub.qs = { 'sqrl': $('#sqrlLink').data('sqrl-id') };
    hub.client.login = function() {
        $('#loginFormForm').submit();
    };

    hub.stateChanged = function(state) {
        var stateConversion = { 0: 'connecting', 1: 'connected', 2: 'reconnecting', 4: 'disconnected' };
        console.log('SignalR state changed from: ' + stateConversion[state.oldState]
         + ' to: ' + stateConversion[state.newState]);
    };
    
    $.connection.hub.start().done(function() {
        $('#loginButton').hide();
    });
});