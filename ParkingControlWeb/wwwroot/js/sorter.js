$(document).ready(function () {
    $('#dateOrderSelect').on('change', function () {
        var selectedSort = $(this).val();
        if (selectedSort === 'newerToOlder') {
            sortNewerToOlder();
        } else if (selectedSort === 'olderToNewer') {
            sortOlderToNewer();
        }
    });

    $('#timePeriodSelect').on('change', function () {
        var selectedPeriod = $(this).val();
        var rows = $('tbody tr');
        if (selectedPeriod === 'today') {
            filterByToday(rows);
        } else if (selectedPeriod === 'yesterday') {
            filterByYesterday(rows);
        } else if (selectedPeriod === 'last100') {
            filterLast100(rows);
        } else if (selectedPeriod === 'thisYear') {
            filterByThisYear(rows);
        } else if (selectedPeriod === 'thisMonth') {
            filterByThisMonth(rows);
        } else if (selectedPeriod === 'thisWeek') {
            filterByThisWeek(rows);
        } else if (selectedPeriod === 'all') {
            showAllRows(rows);
        }
    });
});

function sortNewerToOlder() {
    var rows = $('tbody tr');
    rows.sort(function (a, b) {
        var dateA = new Date($(a).data('date'));
        var dateB = new Date($(b).data('date'));
        return dateB - dateA;
    });
    $('tbody').empty().append(rows);
}

function sortOlderToNewer() {
    var rows = $('tbody tr');
    rows.sort(function (a, b) {
        var dateA = new Date($(a).data('date'));
        var dateB = new Date($(b).data('date'));
        return dateA - dateB;
    });
    $('tbody').empty().append(rows);
}

function filterByToday(rows) {
    var today = new Date();
    var todayDate = today.toISOString().slice(0, 10);
    rows.hide();
    rows.filter('[data-date="' + todayDate + '"]').show();
}

function filterByYesterday(rows) {
    var yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    var yesterdayDate = yesterday.toISOString().slice(0, 10);
    rows.hide();
    rows.filter('[data-date="' + yesterdayDate + '"]').show();
}

function filterLast100(rows) {
    rows.show();
    rows.slice(100).hide();
}

function filterByThisYear(rows) {
    var currentYear = new Date().getFullYear();
    rows.hide();
    rows.filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate.getFullYear() === currentYear;
    }).show();
}

function filterByThisMonth(rows) {
    var currentYear = new Date().getFullYear();
    var currentMonth = new Date().getMonth() + 1; // Month is zero-based
    rows.hide();
    rows.filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate.getFullYear() === currentYear && rowDate.getMonth() + 1 === currentMonth;
    }).show();
}

function filterByThisWeek(rows) {
    var today = new Date();
    var currentYear = today.getFullYear();
    var currentMonth = today.getMonth() + 1; // Month is zero-based
    var currentDate = today.getDate();
    var currentDayOfWeek = today.getDay();
    var weekStartDate = new Date(currentYear, currentMonth - 1, currentDate - currentDayOfWeek);
    var weekEndDate = new Date(currentYear, currentMonth - 1, currentDate - currentDayOfWeek + 6);
    rows.hide();
    rows.filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate >= weekStartDate && rowDate <= weekEndDate;
    }).show();
}

function showAllRows(rows) {
    rows.show();
}
