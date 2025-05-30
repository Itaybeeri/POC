package main

import (
	"encoding/json"
	"net/http"
	"net/http/httputil"
	"net/url"
	"os"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
)

type Route struct {
	Path   string `json:"path"`
	Target string `json:"target"`
}

type Config struct {
	Port   string  `json:"port"`
	Routes []Route `json:"routes"`
}

func LoadConfig(path string) (*Config, error) {
	file, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}
	var cfg Config
	err = json.Unmarshal(file, &cfg)
	if err != nil {
		return nil, err
	}
	return &cfg, nil
}

func main() {
	e := echo.New()

	// Middleware: Logging, recovery
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())

	// Load config
	cfg, err := LoadConfig("config.json")
	if err != nil {
		e.Logger.Fatal("Failed to load config: ", err)
	}

	// Health check
	e.GET("/ping", func(c echo.Context) error {
		return c.String(http.StatusOK, "pong")
	})

	// Register reverse proxy routes
	for _, route := range cfg.Routes {
		e.Any(route.Path, ReverseProxy(route.Target))
	}

	// Start server
	port := cfg.Port
	if port == "" {
		port = "8080"
	}
	e.Logger.Fatal(e.Start(":" + port))
}

// ReverseProxy creates a handler to proxy requests to a target
func ReverseProxy(target string) echo.HandlerFunc {
	targetURL, err := url.Parse(target)
	if err != nil {
		panic("Invalid proxy target URL")
	}

	return func(c echo.Context) error {
		proxy := httputil.NewSingleHostReverseProxy(targetURL)

		// Set custom director inside the handler to access Echo context
		originalDirector := proxy.Director
		proxy.Director = func(req *http.Request) {
			originalDirector(req)
			req.URL.Scheme = targetURL.Scheme
			req.URL.Host = targetURL.Host
			req.URL.Path = c.Request().URL.Path // preserve full path
			req.Host = targetURL.Host
		}

		proxy.ServeHTTP(c.Response(), c.Request())
		return nil
	}
}
