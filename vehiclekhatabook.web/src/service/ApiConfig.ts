"use server";

import { parse } from "cookie";
// import { decrypt } from "@/utils/cookies/cookieEncryption";
import { toast } from "react-toastify";
import { cookies } from "next/headers";

export async function getAdminToken() {
  const cookieStore = await cookies();
  const encryptedAdminData: any = cookieStore.get("session");

  if (!encryptedAdminData) {
    //console.error("No admin token found in cookies");
    return null;
  }

//   const decryptedAdminData = decrypt(encryptedAdminData);

//   if (!decryptedAdminData) {
//     console.error("Failed to decrypt admin token");
//     return null;
//   }

//   const token = JSON.parse(decryptedAdminData).token;

//   return token;
}

export async function handleApiError(error: any) {
  console.error("API Error:", error);
  if (error.response && error.response.data) {
    showErrorToast(error.response.data.message || "An error occurred");
  } else {
    showErrorToast("An unexpected error occurred");
  }
}

const showErrorToast = (message: string): void => {
  toast.error(message, {
    position: "top-right",
    autoClose: 5000,
  });
};