import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";
import { decrypt } from "@/utils/cookies/cookieEncryption";

export function middleware(request: NextRequest) {
  const session = request.cookies.get("session")?.value;

  // Check if session exists and is valid
  if (session) {
    try {
      const userData = decrypt(session); // Decrypt the session data to check its validity
      if (userData) {
        return NextResponse.next(); // Allow the request to proceed
      }
    } catch (error) {
      console.error("Error decrypting session:", error);
    }
  }

  // If no valid session, redirect to login page
  const url = request.nextUrl.clone();
  url.pathname = "/login";
  return NextResponse.redirect(url);
}

// Configure the matcher for specific protected paths
export const config = {
  matcher: [
    "/language-management",
    "/vehicle-type-management",
    "/income-category-management",
    "/expense-category",
    "/application-configuration",
    "/",

  ],
};
