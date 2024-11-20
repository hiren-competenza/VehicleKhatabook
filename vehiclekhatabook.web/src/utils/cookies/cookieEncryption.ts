import { SECRET_KEY } from "../../../../../Bonobo-web-main/Bonobo-web-main/src/config/configKeys";
import CryptoJS from "crypto-js";

// if (!SECRET_KEY) {
//   throw new Error("Encryption key is not defined in environment variables");
// }

export function encrypt(data: any) {
  return CryptoJS.AES.encrypt(data, SECRET_KEY as string).toString();
}

// export function decrypt(data: any) {
//   const bytes = CryptoJS.AES.decrypt(data, SECRET_KEY as string);
//   return bytes.toString(CryptoJS.enc.Utf8);
// }

export function decrypt(data: any) {
  try {
    if (typeof data === "object") {
      const bytes = CryptoJS.AES.decrypt(data.value, SECRET_KEY as string);
      const decryptedData = bytes.toString(CryptoJS.enc.Utf8);

      return decryptedData;
    } else {
      const bytes = CryptoJS.AES.decrypt(data, SECRET_KEY as string);
      const decryptedData = bytes.toString(CryptoJS.enc.Utf8);

      return decryptedData;
    }
  } catch (error) {
    console.error("Error decrypting data:", error);
    return error as string;
  }
}
