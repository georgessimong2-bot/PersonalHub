window.authStorage = {
    set: function (key, value) {
        localStorage.setItem(key, value);
        console.log("Token saved to localStorage. Key: " + key + ", Length: " + value.length);
    },
    get: function (key) {
        const value = localStorage.getItem(key);
        console.log("Token retrieved from localStorage. Key: " + key + ", Found: " + (value !== null) + ", Length: " + (value?.length || 0));
        return value;
    },
    remove: function (key) {
        localStorage.removeItem(key);
        console.log("Token removed from localStorage. Key: " + key);
    }
};