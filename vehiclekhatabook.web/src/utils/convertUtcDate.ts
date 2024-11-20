const convertUtcDate = (utcDate: any) => {
  const moment = require("moment-timezone");

  const localTimezone = moment.tz.guess();
  let date = moment.utc(utcDate).tz(localTimezone);

  const newDate = date.format("Do MMM, YYYY");
  const newTime = date.format("LT");

  return newDate + ", " + newTime;
};

export default convertUtcDate;
