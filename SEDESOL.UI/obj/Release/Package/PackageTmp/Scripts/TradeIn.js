
function CheckAllActivitiesAnswers(obj)
{
    var questions = $("td[id^='Activity_'");
    for (j=0; j< questions.length; j++)
    {
        var radioButtons = document.getElementsByName(questions[j].id);
        var oneSelected = false;
        for (i=0; i < radioButtons.length; i++)
        {
            if (radioButtons[i].checked)
            {
                oneSelected = true; 
            }
        }
        if (!oneSelected)
        {
            obj.title = '<b>' + $('#Title_' + questions[j].id)[0].innerText + '</b>';
            obj.question = '<b>' + $('#Question_' + questions[j].id)[0].innerText + '</b>';
            return false; 
        }
    }
    return true; 
}

function CheckNetwork()
{
    var radioButtons = document.getElementsByName('Network');
    var oneSelected = false;
    for (i = 0; i < radioButtons.length; i++) {
        if (radioButtons[i].checked) {
            oneSelected = true;
        }
    }
    return oneSelected;
}

function CheckPromotions(obj) {
    var valid = true;
    $('input[type=checkbox][name=promotion]:checked').each(function () {
        if ($(this).attr('data-promotype') == 'B') {
            if ($(this).parent().parent().find('select').find(':selected').val() == null || $(this).parent().parent().find('select').find(':selected').val() == "") {
                obj.name = '<br>'+ labels[0] +'<strong>' + $(this).attr('data-promoname') + '</strong>'+labels[1]+ ': ';
                obj.msg = "<br />"+labels[2];
                valid = false;
            }
        }
        if ($(this).attr('data-promotype') == 'C') {
            if ($(this).attr('data-promotionid') == null || $(this).attr('data-promotionid') == "") {
                obj.name = '<br>' + labels[3] + ': ';
                obj.msg = "<br />" + labels[4];
                valid = false;
            }
        }
    });

    return valid;
}

function GetSelectedValues()
{
    var questions = $("td[id^='Activity_'");
    var selectedAnswers = "{'Activities':[";
    for (j = 0; j < questions.length; j++) {
        
        selectedAnswers += "{'ActivityId':'" + String(questions[j].id) + "',"

        var radioButtons = document.getElementsByName(questions[j].id);
        for (i = 0; i < radioButtons.length; i++) {
            if (radioButtons[i].checked) {
                selectedAnswers += "'AnswerId':'" + String(radioButtons[i].id) + "','Grading':'" + String(radioButtons[i].getAttribute("data-grading")) + "'},"
            }
        }
    }
    if (questions.length > 0)
    {
        selectedAnswers = selectedAnswers.substring(0, selectedAnswers.length-1)
    }
    selectedAnswers += "],";

    selectedAnswers += "'Promotions':[";

    $('input[type=checkbox][name=promotion]:checked').each(function () {
        //Id, Name, IsPercentage, TotalBonus, ModelSelectId
        selectedAnswers += "{'Id':'" + $(this).attr('data-promotionid') + "',";
        selectedAnswers += "'Name':'" + $(this).attr('data-promoname') + "',";
        if ($(this).attr('data-ispercentage') == 'True')
            selectedAnswers += "'IsPercentage': true,";
        else
            selectedAnswers += "'IsPercentage': false,";
        selectedAnswers += "'TotalBonus':'" + $(this).attr('data-bonus') + "',";
        selectedAnswers += "'PromotionCustomerId':'" + $(this).attr('data-customerid') + "'";
        if (!($(this).parent().parent().find('select').find(':selected').val() == null || $(this).parent().parent().find('select').find(':selected').val() == "")) {
            selectedAnswers += ",'ModelSelectId':'" + $(this).parent().parent().find('select').find(':selected').val()+ "'";
        }
        selectedAnswers += "},"
    });
    if ($('input[type=checkbox][name=promotion]:checked').length > 0) {
        selectedAnswers = selectedAnswers.substring(0, selectedAnswers.length - 1)
    }
    selectedAnswers += "],";

    var radioButtons = document.getElementsByName('Network');
    for (i = 0; i < radioButtons.length; i++) {
        if (radioButtons[i].checked) {
            selectedAnswers += "'NetworkId':'" + radioButtons[i].value + "'}";
        }
    }
    
    //alert(selectedAnswers);

    return selectedAnswers;

}