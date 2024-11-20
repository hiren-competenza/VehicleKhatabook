const fs = require('fs');
const https = require('https');
const http = require('http');
const next = require('next');

const dev = process.env.NODE_ENV !== 'production';
const app = next({ dev });
const handle = app.getRequestHandler();

if (dev) {
    const httpsOptions = {
        key: fs.readFileSync('./key.pem'),
        cert: fs.readFileSync('./cert.pem'),
    };

    app.prepare().then(() => {
        https
            .createServer(httpsOptions, (req, res) => {
                handle(req, res);
            })
            .listen(3000, (err) => {
                if (err) throw err;
                console.log('> Ready on https://localhost:3000');
            });
    });
} else {
    const httpsOptions = {
        key: fs.readFileSync('./key.pem'),
        cert: fs.readFileSync('./cert.pem'),
    };

    app.prepare().then(() => {
        https
            .createServer(httpsOptions, (req, res) => {
                handle(req, res);
            })
            .listen(3000, (err) => {
                if (err) throw err;
                console.log('> Ready on production https://localhost:3000');
            });
    });

    //// Optionally, redirect HTTP to HTTPS
    //http.createServer((req, res) => {
    //    res.writeHead(301, { Location: `https://${req.headers.host}${req.url}` });
    //    res.end();
    //}).listen(80, () => {
    //    console.log('> HTTP to HTTPS redirect server running on port 80');
    //});
}
