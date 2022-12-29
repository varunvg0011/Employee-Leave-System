function IsValidUsername(username){
    var regexpattern = new RegExp("^[A-Za-z][A-Za-z0-9_]{4,29}$")
    if (username.match(regexpattern)) {
        return true;
    }
    return false;
}


function IsValidFirstName(firstName) {
    var regexpattern = new RegExp("^[a-zA-Z]{1,40}$")
    if (firstName.match(regexpattern)) {
        return true;
    }
    return false;
}

function IsValidLastName(lastName) {
    var regexpattern = new RegExp("^[a-zA-Z]{1,40}$")
    if (lastName.match(regexpattern)) {
        return true;
    }
    return false;
}

function IsValidDesignation(designation) {
    var regexpattern = new RegExp("^[a-zA-Z]{1,}(?: [a-zA-Z]+){0,2}$")
    if (designation.match(regexpattern)) {
        return true;
    }
    return false;
}

function IsValidPassword(password) {
    var regexpattern = new RegExp("^[a-zA-Z0-9+_.-@]{1,40}$")
    if (password.match(regexpattern)) {
        return true;
    }
    return false;
}

