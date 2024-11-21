"use client";
import { useState, Suspense } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import Cookies from "js-cookie";
import { encrypt } from "@/utils/cookies/cookieEncryption";
import {
  Button,
  Card,
  CardBody,
  CardSubtitle,
  CardTitle,
  Col,
  Form,
  FormGroup,
  Input,
  Label,
  Row,
  Alert, // Import Alert component
} from "reactstrap";
import Link from "next/link";
import { getAdminData } from "@/service/admin.service";

function LoginComponent() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const returnUrl = searchParams.get("returnUrl") || "/";
  const [mobileNumber, setEmail] = useState("");
  const [passwordHash, setPassword] = useState("");
  const [loginError, setLoginError] = useState(false); // Add login error state

  const handleLogin = async (e:any) => {
    e.preventDefault();
    try {
      const res = await getAdminData({ mobileNumber, passwordHash });
      if (!res?.data) {
        setLoginError(true);
        return;
      }
  
      const encryptedUser = encrypt(JSON.stringify(res.data));
      Cookies.set("session", encryptedUser, { expires: 1, secure: true });
  
      router.push(returnUrl);
    } catch (e) {
      setLoginError(true);
    }
  };

  return (
    <Row className="align-items-center justify-content-center g-0 min-vh-100">
      <Col xxl={4} lg={6} md={8} xs={12} className="py-8 py-xl-0">
        <h1 className="d-flex justify-content-between align-items-baseline">
          Welcome!, Admin{" "}
        </h1>

        <Card className="smooth-shadow-md border-primary">
          <CardBody className="p-5">
            <div className="pb-4">
              <CardTitle tag="h1" className="text-primary">
                Vehicle KhataBook
              </CardTitle>
              <CardSubtitle className="mb-2 text-muted" tag="h6">
                Please enter your user information.
              </CardSubtitle>
            </div>

            {/* Error alert shown conditionally */}
            {loginError && (
              <Alert color="danger" toggle={() => setLoginError(false)}>
                Incorrect mobile number or password.
              </Alert>
            )}

            <Form onSubmit={handleLogin}>
              <FormGroup className="mb-3">
                <Label for="Email">Mobile Number</Label>
                <Input
                  id="mobileNumber"
                  className="bg-light"
                  type="number"
                  name="mobileNumber"
                  placeholder="Enter your Mobile  Number"
                  value={mobileNumber}
                  onChange={(e) => setEmail(e.target.value)}
                  maxLength={50}
                  required
                />
              </FormGroup>
              <FormGroup className="mb-3">
                <Label for="Password">Password</Label>
                <Input
                  id="Password"
                  name="password"
                  className="bg-light"
                  placeholder="Enter your password"
                  type="password"
                  maxLength={50}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </FormGroup>
              
              <FormGroup className="d-grid">
                <Button className="btn-primary" type="submit">
                  Login
                </Button>
              </FormGroup>
            </Form>
          </CardBody>
        </Card>
      </Col>
    </Row>
  );
}

export default function Login() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <LoginComponent />
    </Suspense>
  );
}
