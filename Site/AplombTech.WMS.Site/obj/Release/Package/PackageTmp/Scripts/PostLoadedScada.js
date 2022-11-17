var graphInterval;
var unit = parseInt($('#ltvalue').text());
var height = parseInt($('#waterlevel').css('height'));
var margin = parseInt($('#waterlevel').css('margin-top'));

var displayedHeightValue = height - (100 - (1.35 * unit));
var disPlayedMarginValue = margin + (100 - (1.35 * unit));

if (displayedHeightValue > 125)
    displayedHeightValue = 125;
if (disPlayedMarginValue < -255)
    disPlayedMarginValue = -255;
$("#waterlevel").css("background-color", "#2fcff4");
$("#waterlevel").css("height", displayedHeightValue + "px");
$("#waterlevel").css("margin-top", disPlayedMarginValue + "px");


//$('#motorswitch').on('click', scadModule.MotorSwitchFunction());
//$('span').on('click', scadModule.ScadaSpanClick());

scadModule.Init();