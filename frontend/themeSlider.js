(function () {
  function hexToRgb(hex) {
    const m = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    if (!m) return [0, 0, 0];
    return [parseInt(m[1], 16), parseInt(m[2], 16), parseInt(m[3], 16)];
  }

  function rgbToHex(r, g, b) {
    const toHex = (v) => v.toString(16).padStart(2, "0");
    return `#${toHex(Math.max(0, Math.min(255, Math.round(r))))}${toHex(
      Math.max(0, Math.min(255, Math.round(g)))
    )}${toHex(Math.max(0, Math.min(255, Math.round(b))))}`;
  }

  function mix(a, b, t) {
    return a + (b - a) * t;
  }

  function mixHex(hexA, hexB, t) {
    const A = hexToRgb(hexA);
    const B = hexToRgb(hexB);
    return rgbToHex(mix(A[0], B[0], t), mix(A[1], B[1], t), mix(A[2], B[2], t));
  }

  const light = {
    bg: "#f7f7f7",
    fg: "#1f1f1f",
    muted: "#9aa0a6",
    card: "#e9eaee",
    border: "#d7d9df",
  };

  const dark = {
    bg: "#0f1115",
    fg: "#e2e4e9",
    muted: "#7a828a",
    card: "#1a1e24",
    border: "#272b33",
  };

  function applyStrength(strength01) {
    const t = Math.max(0, Math.min(1, strength01));
    const root = document.documentElement.style;
    root.setProperty("--bg", mixHex(light.bg, dark.bg, t));
    root.setProperty("--fg", mixHex(light.fg, dark.fg, t));
    root.setProperty("--muted", mixHex(light.muted, dark.muted, t));
    root.setProperty("--card", mixHex(light.card, dark.card, t));
    root.setProperty("--border", mixHex(light.border, dark.border, t));
  }

  function init() {
    const slider = document.getElementById("mode");
    if (!slider) return;

    const saved = localStorage.getItem("modeStrength");
    let value = typeof saved === "string" ? Number(saved) : NaN;
    if (!Number.isFinite(value)) {
      value = Number(slider.value || 60);
    }

    value = Math.max(0, Math.min(100, value));
    slider.value = String(value);
    applyStrength(value / 100);

    slider.addEventListener("input", () => {
      const v = Math.max(0, Math.min(100, Number(slider.value) || 0));
      applyStrength(v / 100);
      try {
        localStorage.setItem("modeStrength", String(v));
      } catch (_) {}
    });
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
  } else {
    init();
  }
})();
